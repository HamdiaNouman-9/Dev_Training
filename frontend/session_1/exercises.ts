interface Student {
  id: number;
  name: string;
  isEnrolled: boolean;
}

interface Course {
  courseCode: string;
  title: string;
  creditHours: number;
  department: string;          
  type: 'Theory' | 'Lab';       
}

interface Grade {
  readonly studentId: number;   
  readonly courseCode: string;
  marks: number;
  letterGrade: string;
}

interface Instructor {
  id: number;
  name: string;
  courses: string[];           
  officeHours: {               
    day: string;
    time: string;
  };
}

enum SemesterType {
  Fall = 'Fall',
  Spring = 'Spring',
  Summer = 'Summer'
}

interface Semester {
  year: number;
  type: SemesterType;
  startDate: Date;
  endDate: Date;
  isActive: boolean;
}


const student: Student = {
  id: 1,
  name: 'Student1',
  isEnrolled: true
};

const course: Course = {
  courseCode: 'CS301',
  title: 'Data Structures',
  creditHours: 3,
  department:'Computer Science',
  type: 'Theory'
};

const grade: Grade = {
  studentId: 1,
  courseCode: 'CS301',
  marks: 88,
  letterGrade: 'A'
};

const instructor: Instructor = {
  id: 10,
  name: 'Dr. Ahmed',
  courses: ['CS301', 'CS401'],
  officeHours: {
    day: 'Monday',
    time: '2:00 PM'
  }
};

const semester: Semester = {
  year: 2025,
  type: SemesterType.Fall,
  startDate: new Date('2025-09-01'),
  endDate: new Date('2026-01-15'),
  isActive: true
};

function findById<T extends {id:number}>(items:T[],id:number):T|undefined{
    return items.find(item=>item.id==id);
}