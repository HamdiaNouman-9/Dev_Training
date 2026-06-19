CREATE TABLE Departments (
    id INT PRIMARY KEY,
    name VARCHAR(100) NOT NULL
);

CREATE TABLE Employees (
    id INT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    hire_date DATE,
    department_id INT,
    manager_id INT,
    FOREIGN KEY (department_id) REFERENCES Departments(id),
    FOREIGN KEY (manager_id) REFERENCES Employees(id)
);

CREATE TABLE Salaries (
    id INT PRIMARY KEY,
    employee_id INT,
    amount DECIMAL(10,2),
    salary_type VARCHAR(50),
    FOREIGN KEY (employee_id) REFERENCES Employees(id)
);

CREATE TABLE Projects (
    id INT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    start_date DATE,
    end_date DATE
);

CREATE TABLE ProjectAssignments (
    employee_id INT,
    project_id INT,
    hours_logged INT,
    PRIMARY KEY (employee_id, project_id),
    FOREIGN KEY (employee_id) REFERENCES Employees(id),
    FOREIGN KEY (project_id) REFERENCES Projects(id)
);


INSERT INTO Departments (id, name) VALUES
(1, 'dep1'),
(2, 'dep2'),
(3, 'dep3');   


INSERT INTO Employees (id, name, hire_date, department_id, manager_id) VALUES
(1, 'emp1', '2022-01-10', 1, NULL),  
(2, 'emp2', '2022-03-15', 1, 1),
(3, 'emp3', '2023-02-20', 1, 1),
(4, 'emp4', '2023-05-12', 2, NULL), 
(5, 'emp5', '2024-01-05', 2, 4),
(6, 'emp6', '2024-04-18', 2, 4),
(7, 'emp7', '2024-06-01', 1, 1),
(8, 'emp8', '2024-07-22', 1, 1); 


INSERT INTO Salaries (id, employee_id, amount, salary_type) VALUES
(1, 1, 50000, 'monthly'),
(2, 2, 60000, 'monthly'),
(3, 3, 700000, 'annual'),
(4, 4, 55000, 'monthly'),
(5, 5, 800000, 'annual'),
(6, 6, 65000, 'monthly'),
(7, 7, 900000, 'annual');


INSERT INTO Projects (id, name, start_date, end_date) VALUES
(1, 'proj1', '2025-01-01', '2025-06-01'),
(2, 'proj2', '2025-02-01', '2025-07-01'),
(3, 'proj3', '2025-03-01', '2025-08-01');


INSERT INTO ProjectAssignments (employee_id, project_id, hours_logged) VALUES
(1, 1, 120),
(2, 1, 150),
(3, 1, 100),
(4, 1, 90),
(5, 2, 200),
(6, 2, 130),
(7, 3, 80);


--Task 1
SELECT 
    e.name AS employee_name, 
    d.name AS department_name, 
    s.amount, 
    s.salary_type
    FROM employees e JOIN departments d ON  e.department_id=d.id LEFT JOIN salaries s ON e.id=s.employee_id;


--Task 2
SELECT d.id, d.name, COUNT(e.id) AS employee_count 
FROM Departments d LEFT JOIN Employees e ON d.id=e.department_id GROUP BY d.id,d.name;

--Task 3
SELECT e.name, d.name, e.hire_date
FROM Employees e
JOIN Departments d ON e.department_id = d.id
WHERE e.id NOT IN (SELECT employee_id FROM ProjectAssignments)
    
--Task 4
SELECT 
    d.id,
    d.name,
    SUM(s.amount) AS total_salary,
    AVG(s.amount) AS avg_salary,
    COUNT(e.id) AS employee_count
FROM Departments d
JOIN Employees e ON e.department_id = d.id
JOIN Salaries s ON s.employee_id = e.id
GROUP BY d.id, d.name
ORDER BY total_salary DESC

--Task 5
SELECT 
    e.name AS employee_name,
    m.name AS manager_name
FROM Employees e
LEFT JOIN Employees m ON e.manager_id = m.id

--Task 6
SELECT COUNT(pa.employee_id) AS emp_count, SUM(pa.hours_logged) AS total_hours
FROM ProjectAssignments pa
GROUP BY pa.project_id
HAVING COUNT(pa.employee_id) > 3
ORDER BY total_hours DESC

--Task 7
SELECT 
    d.name AS department_name,
    p.name AS project_name,
    COUNT(e.id) AS employee_count
FROM Departments d
CROSS JOIN Projects p
LEFT JOIN ProjectAssignments pa ON pa.project_id = p.id
LEFT JOIN Employees e ON pa.employee_id = e.id AND e.department_id = d.id
GROUP BY d.id, d.name, p.id, p.name


--Task 8
DELIMITER $$

CREATE FUNCTION fn_get_emp_tenure(emp_id INT)
RETURNS INT
DETERMINISTIC
BEGIN
    DECLARE tenure INT; 
    
    SELECT TIMESTAMPDIFF(YEAR, e.hire_date, CURDATE())  
    INTO tenure                                       
    FROM Employees e
    WHERE e.id = emp_id;                             
    
    RETURN tenure;  
END$$

DELIMITER ;

SELECT e.name, fn_get_emp_tenure(e.id) AS tenure_years
FROM Employees e;

--Task 9
DELIMITER $$

CREATE FUNCTION fn_annual_salary(emp_id INT)
RETURNS DECIMAL(10,2)
DETERMINISTIC
BEGIN
    DECLARE salary_amount DECIMAL(10,2);
    DECLARE salary_type VARCHAR(50);
    
    SELECT s.amount, s.salary_type
    INTO salary_amount, salary_type
    FROM Salaries s
    WHERE s.employee_id = emp_id;
    
    IF salary_amount IS NULL THEN
        RETURN 0;
    END IF;
    
    IF salary_type = 'monthly' THEN
        RETURN salary_amount * 12;
    ELSE
        RETURN salary_amount;
    END IF;
    
END$$

DELIMITER ;

SELECT e.name, fn_annual_salary(e.id) AS annual_salary
FROM Employees e;

--Task 10
DELIMITER $$

CREATE PROCEDURE fn_dept_employees(dept_id INT)
BEGIN
    SELECT 
        e.id,
        e.name,
        s.amount AS salary,
        s.salary_type,
        TIMESTAMPDIFF(YEAR, e.hire_date, CURDATE()) AS tenure_years
    FROM Employees e
    LEFT JOIN Salaries s ON s.employee_id = e.id
    WHERE e.department_id = dept_id;
END$$

DELIMITER ;

--Task 11
DELIMITER $$

CREATE PROCEDURE sp_dept_salary_report(
    IN dept_id INT,
    OUT emp_count INT,
    OUT total_salary DECIMAL(10,2),
    OUT avg_salary DECIMAL(10,2),
    OUT top_earner VARCHAR(100)
)
BEGIN
    SELECT e.name, s.amount, s.salary_type
    FROM Employees e
    LEFT JOIN Salaries s ON e.id = s.employee_id
    WHERE e.department_id = dept_id;

    SELECT COUNT(e.id) INTO emp_count
    FROM Employees e
    WHERE e.department_id = dept_id;

    SELECT SUM(s.amount) INTO total_salary
    FROM Salaries s
    JOIN Employees e ON s.employee_id = e.id
    WHERE e.department_id = dept_id;

    SELECT AVG(s.amount) INTO avg_salary
    FROM Salaries s
    JOIN Employees e ON s.employee_id = e.id
    WHERE e.department_id = dept_id;

    SELECT e.name INTO top_earner
    FROM Employees e
    JOIN Salaries s ON e.id = s.employee_id
    WHERE e.department_id = dept_id
    ORDER BY s.amount DESC
    LIMIT 1;

END$$

DELIMITER ;

--Task 12
DELIMITER $$

CREATE PROCEDURE sp_give_raise(
    IN dept_id INT,
    IN raise_percent DECIMAL(5,2)
)
BEGIN

    START TRANSACTION;

        UPDATE Salaries s
        JOIN Employees e ON s.employee_id = e.id
        SET s.amount = s.amount + (s.amount * raise_percent / 100)
        WHERE e.department_id = dept_id;

    COMMIT;

END$$

DELIMITER ;