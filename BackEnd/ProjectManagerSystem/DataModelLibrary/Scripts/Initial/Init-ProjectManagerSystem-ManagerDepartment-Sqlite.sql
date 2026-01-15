-- SQLite schema for ManagerDepartment with hierarchy support.
-- This file is for local dev/testing only and is not wired into runtime.

CREATE TABLE IF NOT EXISTS ManagerDepartment (
  ID INTEGER PRIMARY KEY AUTOINCREMENT,
  ParentDepartmentID INTEGER NULL,
  Name TEXT NOT NULL,
  IsEnable INTEGER NOT NULL,
  CreateTime TEXT NOT NULL,
  UpdateTime TEXT NOT NULL,
  FOREIGN KEY (ParentDepartmentID) REFERENCES ManagerDepartment(ID)
);
