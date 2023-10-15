﻿USE MASTER
GO
IF EXISTS ( SELECT * FROM SYS.DATABASES WHERE NAME = 'QuanLySinhVienChuyenNganh')
	DROP DATABASE QuanLySinhVienChuyenNganh
GO

CREATE DATABASE QuanLySinhVienChuyenNganh
GO

USE QuanLySinhVienChuyenNganh
ALTER DATABASE QuanLySinhVienChuyenNganh COLLATE Vietnamese_CI_AS
--CREATE TABLE
IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE NAME = 'Student')
	DROP TABLE Student
GO
CREATE TABLE Student
(
	StudentID NVARCHAR(20) NOT NULL PRIMARY KEY,
	FullName NVARCHAR(20),
	AverageScore FLOAT,
	FacultyID INT,
	MajorID INT,
	Avata NVARCHAR(255)
)
IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE NAME = 'Faculty')
	DROP TABLE Faculty
GO
CREATE TABLE Faculty
(
	FacultyID INT NOT NULL PRIMARY KEY,
	FacultyName NVARCHAR(200)
)
IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE NAME = 'Major')
	DROP TABLE Major
GO
CREATE TABLE Major
(
	FacultyID INT NOT NULL,
	MajorID INT NOT NULL,
	MajorName NVARCHAR(255) NOT NULL
	primary key(MajorID,FacultyID)
)

ALTER TABLE Student
	ADD CONSTRAINT FK_Student_Faculty FOREIGN KEY(FacultyID) REFERENCES Faculty(FacultyID)
ALTER TABLE Major
	ADD CONSTRAINT FK_Major_Faculty FOREIGN KEY(FacultyID) REFERENCES Faculty(FacultyID)
ALTER TABLE Student
	ADD CONSTRAINT FK_Student_Major FOREIGN KEY(MajorID,FacultyID) REFERENCES Major(MajorID,FacultyID)



INSERT INTO Faculty(FacultyID,FacultyName) VALUES('1','CNTT')
INSERT INTO Faculty(FacultyID,FacultyName) VALUES('2','NNA')
INSERT INTO Faculty(FacultyID,FacultyName) VALUES('3','QTKD')

INSERT INTO Major(FacultyID,MajorID,MajorName) VALUES('1','1','CONG NGHE PHAN MEM')
INSERT INTO Major(FacultyID,MajorID,MajorName) VALUES('2','1','TIENG ANH THUONG MAI')
INSERT INTO Major(FacultyID,MajorID,MajorName) VALUES('1','2','HE THONG THONG TIN')
INSERT INTO Major(FacultyID,MajorID,MajorName) VALUES('2','2','TIENG ANH TRUYEN THONG')
INSERT INTO Major(FacultyID,MajorID,MajorName) VALUES('1','3','AN TOAN THONG TIN')
INSERT INTO Major(FacultyID,MajorID,MajorName) VALUES('3','1','NGANH 1')
INSERT INTO Major(FacultyID,MajorID,MajorName) VALUES('3','2','NGANH 2')

INSERT INTO Student(StudentID,FullName,AverageScore,FacultyID,MajorID,Avata) VALUES('ID1', N'STUDENT 1', '6.7','1','1','ID1')
INSERT INTO Student(StudentID,FullName,AverageScore,FacultyID,MajorID,Avata) VALUES('ID2', N'STUDENT 2', '6.9','3','1','ID2')
INSERT INTO Student(StudentID,FullName,AverageScore,FacultyID,MajorID,Avata) VALUES('ID3', N'STUDENT 3', '10','2','2','ID3')
INSERT INTO Student(StudentID,FullName,AverageScore,FacultyID,MajorID,Avata) VALUES('ID4', N'STUDENT 4', '8','1','3','ID4')
INSERT INTO Student(StudentID,FullName,AverageScore,FacultyID,MajorID,Avata) VALUES('ID5', N'STUDENT 5', '9','2','1','ID5')
INSERT INTO Student(StudentID,FullName,AverageScore,FacultyID,MajorID,Avata) VALUES('ID6', N'STUDENT 6', '1','1','2','ID6')


SELECT * FROM Faculty
SELECT * FROM Student
SELECT * FROM Major