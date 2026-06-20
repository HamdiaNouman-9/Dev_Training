use sys
CREATE TABLE Customers (
    id INT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(100),
    registered_at DATE
);

CREATE TABLE Categories (
    id INT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    parent_id INT,
    FOREIGN KEY (parent_id) REFERENCES Categories(id)
);

CREATE TABLE Products (
    id INT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    price DECIMAL(10,2),
    stock INT,
    category_id INT,
    FOREIGN KEY (category_id) REFERENCES Categories(id)
);

CREATE TABLE Orders (
    id INT PRIMARY KEY,
    customer_id INT,
    order_date DATE,
    FOREIGN KEY (customer_id) REFERENCES Customers(id)
);

CREATE TABLE OrderItems (
    order_id INT,
    product_id INT,
    quantity INT,
    unit_price DECIMAL(10,2),
    PRIMARY KEY (order_id, product_id),
    FOREIGN KEY (order_id) REFERENCES Orders(id),
    FOREIGN KEY (product_id) REFERENCES Products(id)
);

CREATE TABLE Payments (
    id INT PRIMARY KEY,
    order_id INT,
    payment_date DATE,
    status VARCHAR(20) CHECK (status IN ('paid', 'unpaid')),
    FOREIGN KEY (order_id) REFERENCES Orders(id)
);

INSERT INTO Customers VALUES
(1, 'Customer1', 'customer1@email.com', '2024-01-01'),
(2, 'Customer2', 'customer2@email.com', '2024-01-05'),
(3, 'Customer3', 'customer3@email.com', '2024-02-01'),
(4, 'Customer4', 'customer4@email.com', '2024-02-15'),
(5, 'Customer5', 'customer5@email.com', '2024-03-01');

INSERT INTO Categories VALUES
(1, 'Category1', NULL),
(2, 'Category2', 1),
(3, 'Category3', 2),
(4, 'Category4', NULL),
(5, 'Category5', 4);

INSERT INTO Products VALUES
(1, 'Product1', 15000.00, 50, 1),
(2, 'Product2', 12000.00, 30, 1),
(3, 'Product3', 2000.00, 200, 2),
(4, 'Product4', 5000.00, 100, 2),
(5, 'Product5', 11000.00, 20, 3);

INSERT INTO Orders VALUES
(1, 1, '2024-03-01'),
(2, 1, '2024-03-15'),
(3, 2, '2024-04-01'),
(4, 2, '2024-04-10'),
(5, 2, '2024-04-20'),
(6, 2, '2024-05-01'),  
(7, 3, '2024-05-10'),
(8, 4, '2024-05-15'),
(9, 5, '2024-06-01');

INSERT INTO OrderItems VALUES
(1, 1, 1, 15000.00),
(1, 3, 2, 2000.00),
(2, 2, 1, 12000.00),
(3, 1, 1, 15000.00),
(4, 3, 3, 2000.00),
(5, 4, 1, 5000.00),
(6, 5, 1, 11000.00),
(7, 2, 2, 12000.00),
(8, 1, 1, 15000.00),
(9, 4, 2, 5000.00);

INSERT INTO Payments VALUES
(1, 1, '2024-03-02', 'paid'),
(2, 2, '2024-03-16', 'paid'),
(3, 3, NULL, 'unpaid'),   
(4, 4, '2024-04-11', 'paid'),
(5, 5, NULL, 'unpaid'),  
(6, 6, '2024-05-02', 'paid'),
(7, 7, '2024-05-11', 'paid'),
(8, 8, NULL, 'unpaid'),
(9, 9, '2024-06-02', 'paid');

--Task 1
SELECT 
    c.id,
    c.name,
    c.email,
    COUNT(o.id) AS total_orders,
    SUM(oi.unit_price * oi.quantity) AS total_spent
FROM Customers c
LEFT JOIN Orders o ON c.id = o.customer_id
LEFT JOIN OrderItems oi ON o.id = oi.order_id
GROUP BY c.id, c.name, c.email;

--Task 2
SELECT 
    c.name,
    COUNT(o.id) AS total_orders,
    SUM(IF(p.status = 'unpaid', 1, 0)) AS unpaid_orders
FROM Customers c
JOIN Orders o ON c.id = o.customer_id
JOIN Payments p ON o.id = p.order_id
GROUP BY c.id, c.name
HAVING COUNT(o.id) > 3
AND SUM(IF(p.status = 'unpaid', 1, 0)) >= 1;

--Task 3
SELECT 
    p.name,
    COUNT(DISTINCT o.customer_id) AS distinct_customers,
    SUM(oi.quantity) AS total_units,
    SUM(oi.quantity * oi.unit_price) AS total_revenue
FROM Products p
JOIN OrderItems oi ON p.id = oi.product_id
JOIN Orders o ON o.id = oi.order_id
GROUP BY p.id, p.name
ORDER BY total_revenue DESC;

--Task 4
SELECT 
    o.id AS order_id,
    o.customer_id,
    p.status,
    CASE WHEN p.status = 'unpaid' THEN NULL
         ELSE DATEDIFF(p.payment_date, o.order_date)
    END AS days
FROM Orders o
JOIN Payments p ON o.id = p.order_id;

--Task 5
WITH RankedProducts AS
(
    SELECT 
        p.id,
        p.name AS product_name,
        c.name AS category_name,
        SUM(oi.quantity) AS total_units,
        ROW_NUMBER() OVER (PARTITION BY p.category_id ORDER BY SUM(oi.quantity) DESC) AS rank_num
    FROM Products p
    JOIN Categories c ON p.category_id = c.id
    JOIN OrderItems oi ON p.id = oi.product_id
    GROUP BY p.id, p.name, c.id, c.name, p.category_id
)
SELECT * FROM RankedProducts
WHERE rank_num <= 3;

--Task 6
WITH CustomerSpending AS
(
    SELECT 
        c.id,
        c.name,
        SUM(oi.quantity * oi.unit_price) AS total_spent
    FROM Customers c
    LEFT JOIN Orders o ON c.id = o.customer_id
    LEFT JOIN OrderItems oi ON o.id = oi.order_id
    GROUP BY c.id, c.name
)
SELECT 
    name,
    total_spent,
    CASE 
        WHEN total_spent > (SELECT AVG(total_spent) FROM CustomerSpending) THEN 'Above Average'
        WHEN total_spent = (SELECT AVG(total_spent) FROM CustomerSpending) THEN 'Average'
        ELSE 'Below Average'
    END AS spending_label
FROM CustomerSpending;

--Task 7
WITH MonthlyRevenue AS
(
    SELECT 
        DATE_FORMAT(o.order_date, '%Y-%m') AS month,
        SUM(oi.quantity * oi.unit_price) AS monthly_revenue
    FROM Orders o
    JOIN OrderItems oi ON o.id = oi.order_id
    WHERE o.order_date >= DATE_SUB(CURDATE(), INTERVAL 12 MONTH)
    GROUP BY DATE_FORMAT(o.order_date, '%Y-%m')
)
SELECT 
    month,
    monthly_revenue,
    SUM(monthly_revenue) OVER (ORDER BY month) AS cumulative_total
FROM MonthlyRevenue
ORDER BY month;

--Task 8
WITH FirstOrders AS
(
    SELECT 
        c.id,
        c.name,
        c.registered_at,
        MIN(o.order_date) AS first_order_date
    FROM Customers c
    JOIN Orders o ON c.id = o.customer_id
    GROUP BY c.id, c.name, c.registered_at
)
SELECT 
    name,
    registered_at,
    first_order_date,
    DATEDIFF(first_order_date, registered_at) AS gap
FROM FirstOrders
WHERE DATEDIFF(first_order_date, registered_at) <= 7;

--Task 9
WITH RECURSIVE CategoryCTE AS
(
   SELECT      id,     name,     parent_id,     name AS path    
   FROM Categories WHERE parent_id IS NULL
    UNION ALL
    SELECT 
    c.id,
    c.name,
    c.parent_id,
    CONCAT(cte.path, ' > ', c.name) AS path
FROM Categories c
JOIN CategoryCTE cte ON c.parent_id = cte.id
)
SELECT id, name, path
FROM CategoryCTE

--Task 10
CREATE VIEW vw_customer_order_summary AS
SELECT 
    c.id,
    c.name,
    COUNT(o.id) AS total_orders,
    SUM(oi.quantity * oi.unit_price) AS total_spent,
    MAX(o.order_date) AS last_order_date
FROM Customers c
LEFT JOIN Orders o ON c.id = o.customer_id
LEFT JOIN OrderItems oi ON o.id = oi.order_id
GROUP BY c.id, c.name;

SELECT * 
FROM vw_customer_order_summary
ORDER BY total_spent DESC
LIMIT 10;

--Task 11
EXPLAIN SELECT 
    p.name,
    COUNT(DISTINCT o.customer_id) AS distinct_customers,
    SUM(oi.quantity) AS total_units,
    SUM(oi.quantity * oi.unit_price) AS total_revenue
FROM Products p
JOIN OrderItems oi ON p.id = oi.product_id
JOIN Orders o ON o.id = oi.order_id
GROUP BY p.id, p.name
ORDER BY total_revenue DESC;

CREATE INDEX idx_orderitems_product_id ON OrderItems(product_id);
CREATE INDEX idx_orders_customer_id ON Orders(customer_id);

EXPLAIN SELECT
    p.name,
    COUNT(DISTINCT o.customer_id) AS distinct_customers,
    SUM(oi.quantity) AS total_units,
    SUM(oi.quantity * oi.unit_price) AS total_revenue
FROM Products p
JOIN OrderItems oi ON p.id = oi.product_id
JOIN Orders o ON o.id = oi.order_id
GROUP BY p.id, p.name
ORDER BY total_revenue DESC;

--Task 12
CREATE FUNCTION fn_customer_lifetime_value(cust_id INT)
RETURNS DECIMAL(10,2)
DETERMINISTIC
BEGIN
    DECLARE total DECIMAL(10,2);
    
    SELECT SUM(oi.quantity * oi.unit_price)
    INTO total
    FROM Orders o
    JOIN OrderItems oi ON o.id = oi.order_id
    JOIN Payments p ON o.id = p.order_id
    WHERE o.customer_id = cust_id
    AND p.status = 'paid';
    
    IF total IS NULL THEN
        RETURN 0;
    END IF;
    
    RETURN total;
END;

SELECT 
    c.name,
    fn_customer_lifetime_value(c.id) AS lifetime_value
FROM Customers c;

--Task 13
CREATE FUNCTION fn_order_discount(order_id INT)
RETURNS DECIMAL(10,2)
DETERMINISTIC
BEGIN
    DECLARE total DECIMAL(10,2);
    
    SELECT SUM(oi.quantity * oi.unit_price)
    INTO total
    FROM Orders o
    JOIN OrderItems oi ON o.id = oi.order_id
    WHERE o.id = order_id;
    
    IF total IS NULL THEN 
        RETURN 0;
    END IF;
    
    IF total > 10000 THEN 
        RETURN total - (total * 0.10);
    ELSEIF total > 5000 THEN 
        RETURN total - (total * 0.05);
    ELSE 
        RETURN total;
    END IF;
    
END;

SELECT 
    o.id AS order_id,
    fn_order_discount(o.id) AS discounted_total
FROM Orders o;

--Task 14
CREATE PROCEDURE fn_orders_by_date_range(
    IN start_date DATE,
    IN end_date DATE
)
BEGIN
    SELECT 
        o.id AS order_id,
        c.name AS customer_name,
        COUNT(oi.product_id) AS total_items,
        SUM(oi.quantity * oi.unit_price) AS order_total
    FROM Orders o
    JOIN Customers c ON o.customer_id = c.id
    JOIN OrderItems oi ON o.id = oi.order_id
    WHERE o.order_date BETWEEN start_date AND end_date
    GROUP BY o.id, c.name;
END;

CALL fn_orders_by_date_range('2024-03-01', '2024-04-30');

--TASK 15
CREATE PROCEDURE sp_place_order(
    IN p_customer_id INT,
    IN p_product_id INT,
    IN p_quantity INT
)
BEGIN
    DECLARE p_price DECIMAL(10,2);
    DECLARE p_stock INT;
    DECLARE p_order_id INT;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        SELECT 'Error occurred, order rolled back' AS error_message;
    END;

    START TRANSACTION;

    SELECT price, stock 
    INTO p_price, p_stock
    FROM Products 
    WHERE id = p_product_id;

    IF p_stock < p_quantity THEN
        SIGNAL SQLSTATE '45000' 
        SET MESSAGE_TEXT = 'Not enough stock';
    END IF;

    INSERT INTO Orders (customer_id, order_date) 
    VALUES (p_customer_id, CURDATE());
    
    SET p_order_id = LAST_INSERT_ID();

    INSERT INTO OrderItems (order_id, product_id, quantity, unit_price)
    VALUES (p_order_id, p_product_id, p_quantity, p_price);

    UPDATE Products 
    SET stock = stock - p_quantity
    WHERE id = p_product_id;

    INSERT INTO Payments (order_id, payment_date, status)
    VALUES (p_order_id, NULL, 'unpaid');

    COMMIT;
    SELECT 'Order placed successfully' AS message;

END;

CALL sp_place_order(1, 1, 2);

SELECT id, name, stock FROM Products WHERE id = 1;

CALL sp_place_order(1, 99, 1);

--Task 16
CREATE PROCEDURE sp_monthly_sales_report(
    IN p_year INT,
    IN p_month INT,
    OUT total_orders INT,
    OUT total_revenue DECIMAL(10,2)
)
BEGIN
    SELECT 
        COUNT(DISTINCT o.id),
        SUM(oi.quantity * oi.unit_price)
    INTO total_orders, total_revenue
    FROM Orders o
    JOIN OrderItems oi ON o.id = oi.order_id
    WHERE YEAR(o.order_date) = p_year
    AND MONTH(o.order_date) = p_month;

    SELECT 
        p.name AS product_name,
        SUM(oi.quantity) AS units_sold
    FROM Products p
    JOIN OrderItems oi ON p.id = oi.product_id
    JOIN Orders o ON oi.order_id = o.id
    WHERE YEAR(o.order_date) = p_year
    AND MONTH(o.order_date) = p_month
    GROUP BY p.id, p.name
    ORDER BY units_sold DESC
    LIMIT 5;

    SELECT 
        c.name AS customer_name,
        SUM(oi.quantity * oi.unit_price) AS total_spent
    FROM Customers c
    JOIN Orders o ON c.id = o.customer_id
    JOIN OrderItems oi ON o.id = oi.order_id
    WHERE YEAR(o.order_date) = p_year
    AND MONTH(o.order_date) = p_month
    GROUP BY c.id, c.name
    ORDER BY total_spent DESC
    LIMIT 3;

END;

SET @orders = 0;
SET @revenue = 0;

CALL sp_monthly_sales_report(2024, 3, @orders, @revenue);

SELECT @orders AS total_orders, @revenue AS total_revenue;

--Task 17
CREATE TABLE RestockAlerts (
    id INT PRIMARY KEY AUTO_INCREMENT,
    product_name VARCHAR(100),
    current_stock INT,
    alert_time DATETIME
);

UPDATE Products SET stock = 5 WHERE id = 5;

CREATE PROCEDURE sp_restock_alerts()
BEGIN
    DECLARE done INT DEFAULT 0;
    DECLARE p_name VARCHAR(100);
    DECLARE p_stock INT;

    DECLARE product_cursor CURSOR FOR
        SELECT name, stock 
        FROM Products 
        WHERE stock < 10;

    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = 1;

    OPEN product_cursor;

    restock_loop: LOOP
        FETCH product_cursor INTO p_name, p_stock;
        
        IF done = 1 THEN 
            LEAVE restock_loop;
        END IF;

        INSERT INTO RestockAlerts (product_name, current_stock, alert_time)
        VALUES (p_name, p_stock, NOW());

    END LOOP restock_loop;

    CLOSE product_cursor;

    SELECT * FROM RestockAlerts;
END;

CALL sp_restock_alerts();

--Task 18
CREATE TABLE MonthlySalesAudit (
    id INT PRIMARY KEY AUTO_INCREMENT,
    year_num INT,
    month_num INT,
    total_orders INT,
    total_revenue DECIMAL(10,2)
);

CREATE PROCEDURE sp_populate_monthly_audit()
BEGIN
    DECLARE v_month INT DEFAULT 1;
    DECLARE v_year INT DEFAULT 2024;
    DECLARE v_orders INT;
    DECLARE v_revenue DECIMAL(10,2);

    WHILE v_month <= 12 DO

        SELECT 
            COUNT(DISTINCT o.id),
            COALESCE(SUM(oi.quantity * oi.unit_price), 0)
        INTO v_orders, v_revenue
        FROM Orders o
        JOIN OrderItems oi ON o.id = oi.order_id
        WHERE YEAR(o.order_date) = v_year
        AND MONTH(o.order_date) = v_month;

        INSERT INTO MonthlySalesAudit (year_num, month_num, total_orders, total_revenue)
        VALUES (v_year, v_month, COALESCE(v_orders, 0), COALESCE(v_revenue, 0));

        SET v_month = v_month + 1;

    END WHILE;

    SELECT * FROM MonthlySalesAudit
    ORDER BY total_revenue DESC;

END;

CALL sp_populate_monthly_audit();

SELECT 
    year_num,
    month_num,
    total_orders,
    total_revenue
FROM MonthlySalesAudit
ORDER BY total_revenue DESC
LIMIT 1;