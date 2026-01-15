
-- public."AtomMenu" definition
DROP TABLE "AtomMenu";
CREATE TABLE "AtomMenu" (
	"ID" int2 NOT NULL,
	"Description" bpchar(30) NOT NULL,
	CONSTRAINT "AtomMenu_pkey" PRIMARY KEY ("ID")
);

-- public."Manager" definition
DROP TABLE "Manager";
CREATE TABLE "Manager" (
	"ID" serial4 NOT NULL,
	"Account" bpchar(50) NOT NULL,
	"Password" bpchar(50) NOT NULL,
	"Email" bpchar(100) NOT NULL,
	"Name" bpchar(50) NOT NULL,
	"ManagerRegionID" int2 NOT NULL,
	"ManagerDepartmentID" int2 NOT NULL,
	"IsEnable" bool DEFAULT true NOT NULL,
	"UpdateTime" timestamptz(3) DEFAULT now() NOT NULL,
	"CreateTime" timestamptz(3) DEFAULT now() NOT NULL,
	CONSTRAINT "Manager_Account_key" UNIQUE ("Account"),
	CONSTRAINT "Manager_pkey" PRIMARY KEY ("ID")
);

-- public."ManagerDepartment" definition
DROP TABLE "ManagerDepartment";
CREATE TABLE "ManagerDepartment" (
	"ID" int2 NOT NULL,
	"Name" bpchar(20) NOT NULL,
	"IsEnable" bool NOT NULL,
	"CreateTime" timestamptz(3) NOT NULL,
	"UpdateTime" timestamptz(3) NOT NULL,
	CONSTRAINT "ManagerDepartment_pkey" PRIMARY KEY ("ID")
);

-- public."ManagerMenuPermission" definition
DROP TABLE "ManagerMenuPermission";
CREATE TABLE "ManagerMenuPermission" (
	"ManagerID" int4 NOT NULL,
	"AtomMenuID" int2 NOT NULL,
	"KindID" int2 NOT NULL,
	CONSTRAINT "ManagerMenuPermission_pkey" PRIMARY KEY ("AtomMenuID", "ManagerID")
);

-- public."ManagerRegion" definition
DROP TABLE "ManagerRegion";
CREATE TABLE "ManagerRegion" (
	"ID" int2 NOT NULL,
	"Name" bpchar(20) NOT NULL,
	"IsEnable" bool NOT NULL,
	"CreateTime" timestamptz(3) NOT NULL,
	"UpdateTime" timestamptz(3) NOT NULL,
	CONSTRAINT "ManagerRegion_pkey" PRIMARY KEY ("ID")
);
