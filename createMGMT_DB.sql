-- ResultsMGMTDB creation script
IF DB_ID('createMGMT_DB') IS NOT NULL             
	BEGIN
		PRINT 'Database exists - dropping.';
		
		USE master;		
		ALTER DATABASE createMGMT_DB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
		
		DROP DATABASE createMGMT_DB;
	END

GO

--  If database does not exist, create it.

PRINT 'Creating database.';

CREATE DATABASE createMGMT_DB;

GO

--  Now that an empty database has been created, we will make it the active one.
--  The table creation statements that follow will therefore be executed on the newly created database.

USE createMGMT_DB;

GO



--  **************************************************************************************
--  We now create the tables in the database.
--  **************************************************************************************



--  **************************************************************************************
--  Create the unit table to hold unit information for ResultsMGMTDB.
--  The result table has a foreign key referencing this table.

PRINT 'Creating unit table...';

CREATE TABLE unit
( unitCode			CHAR(7)			NOT NULL	PRIMARY KEY,
  unitTitle			VARCHAR(30)		NOT NULL,
  unitCoordinator	VARCHAR(30)		NOT NULL, 
  unitOutline		NVARCHAR(250)	NULL

  --CONSTRAINT valid_unit_code CHECK (unitCode like '[a-z][a-z][a-z][0-9][0-9][0-9][0-9]')
);


--  **************************************************************************************
--  Create the student table to hold student information. 
--  The result table has a foreign key referencing this table.


PRINT 'Creating student table...';

CREATE TABLE student 
( studentID			CHAR(8)			NOT NULL 	PRIMARY KEY,
  studentName		VARCHAR(30)		NULL,
  studentPhoto		NVARCHAR(250)	NULL,

  CONSTRAINT valid_studentID CHECK (studentID like REPLICATE('[0-9]', 8))
);


--  **************************************************************************************
--  Create the result table to hold result information.

PRINT 'Creating result table...';

CREATE TABLE result
( resultID			INT				NOT NULL 	IDENTITY PRIMARY KEY,
  unitCode			CHAR(7)			NOT NULL, 
  studentID			CHAR(8)			NOT NULL,
  rYear				CHAR(4)			NOT NULL,
  rSemester			CHAR(1)			NOT NULL, 
  ass1				TINYINT			NOT NULL,
  ass2				TINYINT			NOT NULL,
  exam				TINYINT			NOT NULL,
  unitScore	AS ass1 + ass2 + exam,
  grade AS 
    CASE WHEN ass1 + ass2 + exam >= 80 THEN 'HD'
    WHEN ass1 + ass2 + exam >= 70 THEN 'D' 
    WHEN ass1 + ass2 + exam >= 60 THEN 'CR' 
    WHEN ass1 + ass2 + exam >= 50 THEN 'Pass' 
    ELSE 'N'
  END,

  CONSTRAINT unit_code_fk FOREIGN KEY (unitCode) REFERENCES unit(unitCode) ON DELETE CASCADE,
  CONSTRAINT studentID_fk FOREIGN KEY (studentID) REFERENCES student(studentID) ON DELETE CASCADE,
  CONSTRAINT ass1_between_0_and_20 CHECK (ass1 >= 0 and ass1 <=20),
  CONSTRAINT ass2_between_0_and_20 CHECK (ass2 >= 0 and ass2 <=20),
  CONSTRAINT exam_between_0_and_60 CHECK (exam >= 0 and exam <=60),
  CONSTRAINT year_is_4_digits CHECK (rYear like REPLICATE('[0-9]', 4)),
  CONSTRAINT sem_is_1_or_2 CHECK (rSemester like '[1-2]')
);

-- Writing insert statements here
/*
PRINT 'Populating unit table...';

INSERT INTO unit 
VALUES ('aaa1111', 'Systems and Databases', 'Greg Baatard', 'UploadedFiles/UnitOutlines/SystemsAndDatabases.pdf'),
	   ('aaa1211', 'Application Development', 'Naeem Janjua', 'UploadedFiles/UnitOutlines/ApplicationDevelopment.pdf'),
	   ('aaa1212', 'Programming Principles', 'Greg Baatard', 'UploadedFiles/UnitOutlines/ProgrammingPrinciples.pdf'),
	   ('aaa1215', 'Applied Communications', 'Brett Turner', 'UploadedFiles/UnitOutlines/AppliedComunnications.pdf'),
	   ('aaa1266', 'Object Oriented C++', 'Martin Masek', 'UploadedFiles/UnitOutlines/ObjectOrientedCPP.pdf'),
	   ('aaa1217', 'Intelligent Systems', 'Saed Shams', 'UploadedFiles/UnitOutlines/IntelligentSystems.pdf'),
	   ('aaa1311', 'Project Management', 'David Cook', 'UploadedFiles/UnitOutlines/ProjectManagement.pdf');

PRINT 'Populating student table...';

INSERT INTO student 
VALUES ('12345678', 'Peter Peter', 'UploadedFiles/StudentPhotos/FACE1.png'),
	   ('12345671', 'Oto Drahonovsky', 'UploadedFiles/StudentPhotos/FACE2.png'),
	   ('12345633', 'Nelson Pui', 'UploadedFiles/StudentPhotos/FACE3.png'),
	   ('12345666', 'Michael Ibesa', 'UploadedFiles/StudentPhotos/FACE4.png'),
	   ('12345691', 'Kyle Chew', 'UploadedFiles/StudentPhotos/FACE5.png'),
	   ('12345444', 'Sam Samsunder', 'UploadedFiles/StudentPhotos/FACE6.png'),
	   ('12345888', 'Rodrigo Mundez', 'UploadedFiles/StudentPhotos/FACE7.png'),
	   ('12345673', 'Chelsea Kraft', 'UploadedFiles/StudentPhotos/FACE8.png');

PRINT 'Populating student table...';

INSERT INTO result 
VALUES ('aaa1111', '12345678', '2017', '1', 20, 11, 55),
	   ('aaa1111', '12345671', '2017', '2', 11, 15, 44),
	   ('aaa1211', '12345678', '2016', '1', 16, 8, 60),
	   ('aaa1211', '12345673', '2016', '1', 20, 13, 50),
	   ('aaa1211', '12345888', '2015', '1', 12, 17, 41),
	   ('aaa1212', '12345678', '2016', '1', 19, 11, 30),
	   ('aaa1212', '12345444', '2017', '2', 16, 7, 34),
	   ('aaa1212', '12345691', '2017', '1', 6, 11, 37),
	   ('aaa1212', '12345671', '2016', '1', 8, 11, 55),
	   ('aaa1215', '12345666', '2015', '2', 6, 5, 21),
	   ('aaa1215', '12345633', '2016', '2', 3, 7, 12),
	   ('aaa1215', '12345678', '2017', '1', 12, 11, 20),
	   ('aaa1266', '12345691', '2016', '2', 3, 11, 56),
	   ('aaa1266', '12345673', '2016', '1', 7, 19, 42),
	   ('aaa1266', '12345666', '2015', '1', 13, 19, 41),
	   ('aaa1217', '12345673', '2017', '2', 11, 17, 44),
	   ('aaa1217', '12345888', '2016', '2', 17, 19, 57),
	   ('aaa1311', '12345888', '2017', '2', 19, 11, 45),
	   ('aaa1311', '12345671', '2015', '2', 20, 11, 0);
*/
