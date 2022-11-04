create database ERental

use [ERental]

-------------State Table-------------
create table State(
StateId int identity,
StateName varchar(30),
CONSTRAINT PK_State PRIMARY KEY (StateId)
)





-------------City Table-------------

create table City(
CityId int identity , 
CityName varchar(50) ,
StateId int FOREIGN KEY REFERENCES State(StateID),
CONSTRAINT PK_City PRIMARY KEY (CityId)
)

----------Vendor Table------------

create table Vendor (
VendorId int identity , 
UserName varchar(25) ,
FirstName varchar(25) ,
LastName varchar(25),
Address varchar(250) ,
StateId int FOREIGN KEY REFERENCES State(StateId) ,
CityId int FOREIGN KEY REFERENCES City(CityId) ,
PinCode int,
PhoneNo varchar(10) ,
EmailId varchar(50),
CONSTRAINT PK_Vendor PRIMARY KEY (VendorId)
)

--------------Customer Table-----------

create table Customer(
CustomerId int identity ,
UserName varchar(25) , 
FirstName varchar(25), 
LastName varchar(25) , 
StateId int FOREIGN KEY REFERENCES State(StateId) ,
CityId int FOREIGN KEY REFERENCES City(CityId) ,
PinCode int,
PhoneNo varchar(10) , 
EmailId varchar(50) , 
AccountBalance money,
CONSTRAINT PK_Customer PRIMARY KEY (CustomerId)
)

--------------Product table-------------

create table Product(
ProductId int identity , 
ProductName varchar(50) , 
ProductType varchar(50) , 
Description varchar(250) , 
PricePerMonth money  ,
InitialDeposit money,
FreeDelivery bit , 
IsAvailable bit,
VendorId int FOREIGN KEY REFERENCES Vendor(VendorId), 
DeliveryBy varchar(50),
CONSTRAINT PK_Product PRIMARY KEY (ProductId)
)
---------changed varchar(25) to varchar(250)----------
alter table Product alter column DeliveryBy varchar(250)

 ----------inserting values into state table--------
 insert into State values ('Maharashtra')
 insert into State values ('Karnataka')
 insert into State values ('Gujarat')

 select*from State


 ----------inserting values into city table--------

insert into City values ('Mumbai',1)
insert into City values ('Pune',1)
insert into City values ('Bengaluru',2)
insert into City values ('Mangaluru',2)
insert into City values ('Surat',3)
insert into City values ('Ahmedabad',3)



select*from City



------------inserting values into Vendor Table----------

insert into Vendor values ('adani@','Adani','Panday','No11_fifth_street',1,2,639004,9856256535,'adani@gmail.com')
insert into Vendor values ('yasin@','Yasin','Shaikh','Jawahar_Bazar',2,3,639114,8525064585,'yasin@gmail.com')
insert into Vendor values ('kuldeep@','Kuldeep','Sharma','Kamla_Nagar',3,6,400037,8525064568,'kuldeep@gmail.com')

select* from Vendor



-------------inserting values into Product---------------

insert into Product values ('Study_Table','Furniture','Spacious_to_keep_your_Studyessentials',700,1000,'True','True',1,'Deliveryby_after_twodays_of_booking')
insert into Product values ('Cross_Trainer','Fitness','EightLevel_Resistance_control_with_HeartRate_Monitoring',1109,1500,'True','True',2,'Deliveryby_after_twodays_of_booking')

select*from Product


------------inserting values into Customer------------

insert into Customer values ('jaya@','Jaya','Sankari',1,1,639004,8525061987,'jaya@gmail.com',1000)
insert into Customer values ('maha@','Maha','Lakshmi',2,4,639114,9858654525,'maha@gmail.com',0)
insert into Customer values ('reny@','Ranjit','Mishra',3,6,400037,8958658520,'reny@gmail.com',2000)
insert into Customer values ('niz@','Nizam','Khan',1,2,400087,8570061547,'niz@gmail.com',6000)


select*from Customer




