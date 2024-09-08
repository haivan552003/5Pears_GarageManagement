--database
create database nhaxe_datn
go
use nhaxe_datn
go

create table role_task(
	id_role_task int identity(1, 1) primary key,
	[name] nvarchar(250),
	date_create datetime,
	date_update datetime
);

create table roles(
	id_role int identity(1, 1) primary key,
	date_create datetime,
	date_update datetime
);

alter table roles
add name nvarchar(150)

create table role_task_role(
	id_role_task_role int identity(1, 1) primary key,
	date_create datetime,
	date_update datetime,

	id_role_task int foreign key 
		references role_task(id_role_task),
	id_role int foreign key 
		references roles(id_role)
);

create table employees(
	id_emp int identity(1, 1) primary key,
	[user_name] varchar(50),
	pass_word varchar(50),
	full_name nvarchar(250),
	birthday datetime,
	citizen_identity_img nvarchar(max),
	citizen_identity_number varchar(50),
	gender bit,
	is_delete bit,
	date_create datetime,
	date_update datetime,

	id_role int foreign key
		references roles(id_role)
);

create table customer(
	id_cus int identity(1, 1) primary key,
	[user_name] varchar(50),
	pass_word varchar(50),
	full_name nvarchar(250),
	birthday datetime,
	gender bit,
	phone_number varchar(20),
	citizen_identity_img nvarchar(max),
	citizen_identity_number varchar(50),
	driver_license_img nvarchar(max),
	driver_license_number varchar(50),
	is_delete bit,

	id_role int foreign key
		references roles(id_role)
);

create table customer_address(
	id_address int identity(1, 1) primary key,
	[address] nvarchar(500),
	[type] nvarchar(250),
	is_delete bit,
	date_create datetime,
	date_update datetime,

	id_cus int foreign key
		references customer(id_cus)
);

create table guest(
	id_guest int identity(1, 1) primary key,
	full_name nvarchar(250),
	phone_number varchar(20),
	[address] nvarchar(max),
	citizen_identity_img nvarchar(max),
	citizen_identity_number varchar(50),
	driver_license_img nvarchar(max),
	driver_license_number varchar(50),
	is_delete bit,
	date_create datetime,
	date_update datetime,

	id_emp int foreign key
		references employees(id_emp)
);

create table driver(
	id_driver int identity(1, 1) primary key,
	img_driver nvarchar(max),
	full_name nvarchar(250),
	birthday datetime,
	citizen_identity_img nvarchar(max),
	citizen_identity_number varchar(50),
	gender bit,
	driver_license_img nvarchar(max),
	driver_license_number varchar(50),
	[status] nvarchar(250),
	is_delete bit,
	date_create datetime,
	date_update datetime
);

create table news(
	id_news int identity(1, 1) primary key,
	news_img nvarchar(max),
	title nvarchar(500),
	content nvarchar(max),
	date_create datetime,
	date_update datetime,

	id_emp int foreign key
		references employees(id_emp)
);

alter table news
add is_delete bit

create table banner(
	id_banner int identity(1, 1) primary key,
	img_banner nvarchar(max),
	title nvarchar(max),
	date_create datetime,
	date_update datetime,

	id_emp int foreign key
		references employees(id_emp)
);


alter table banner
add [status] bit

create table trip(
	id_trip int identity(1, 1) primary key,
	img_trip nvarchar(max),
	[from] nvarchar(500),
	[to] nvarchar(500),
	distance varchar(100),
	date_create datetime,
	date_update datetime,
	is_delete bit,
	emp_create int foreign key
		references employees(id_emp)
);

create table car(
	id_car int identity(1, 1) primary key,
	car_number varchar(20),
	[type] nvarchar(150),
	brand nvarchar(150),
	color nvarchar(150),
	vehicle_registration_start datetime,
	vehicle_registration_end datetime,
	status_vehicle_registration nvarchar(150),
	[status] nvarchar(150),
	is_delete bit,
	date_create datetime,
	date_update datetime
);

create table car_img(
	id_car_img int identity(1, 1) primary key,
	[name] nvarchar(max),
	is_delete bit,
	date_create datetime,
	date_update datetime,

	id_car int foreign key
		references car(id_car)
);

create table car_seat(
	id_car_seat int identity(1, 1) primary key,
	[name] nvarchar(50),
	is_delete bit,
	date_create datetime,
	date_update datetime,

	id_car int foreign key 
		references car(id_car)
);

create table [location](
	id_location int identity(1, 1) primary key,
	[name] nvarchar(500),
	[address] nvarchar(500),
	phone_number nvarchar(20),
	date_create datetime,
	date_update datetime,
	is_delete bit
);

create table trip_detail(
	id_trip_detail int identity(1, 1) primary key,
	time_start datetime,
	time_end datetime,
	price float,
	voucher float,
	is_delete bit,
	date_create datetime,
	date_update datetime,

	id_trip int foreign key
		references trip(id_trip),
	id_car int foreign key
		references car(id_car),
	id_location int foreign key
		references location(id_location),
	id_driver int foreign key
		references driver(id_driver)
);

create table guest_car(
	id_guest_car int identity(1, 1) primary key,
	date_start datetime,
	date_end datetime,
	price float,
	[status] nvarchar(250),
	is_delete bit,
	date_create datetime,
	date_update datetime,

	id_guest int foreign key
		references guest(id_guest),
	id_emp int foreign key
		references employees(id_emp),
	id_cus int foreign key	
		references customer(id_cus),
	id_car int foreign key
		references car(id_car)
);

create table img_guest_car(
	id_img_guest_car int identity(1, 1) primary key,
	is_delete bit,
	date_create datetime,
	date_update datetime,

	id_guest_car int foreign key
		references guest_car(id_guest_car)
);

create table guest_trip(
	id_guest_trip int identity(1, 1) primary key,
	is_delete bit,
	status nvarchar(150),
	date_create datetime,
	date_update datetime,

	id_guest int foreign key
		references guest(id_guest),
	id_emp int foreign key
		references employees(id_emp),
	id_cus int foreign key	
		references customer(id_cus),
	id_trip_detail int foreign key
		references trip_detail(id_trip_detail)
);

create table seat_guest_trip(
	id_seat_guest_trip int identity(1, 1) primary key,
	is_delete bit,
	status nvarchar(150),
	date_create datetime,
	date_update datetime,

	id_guest_trip int foreign key
		references guest_trip(id_guest_trip),
	id_car_seat int foreign key
		references car_seat(id_car_seat)
);

create table guest_driver(
	id_guest_driver int identity(1, 1) primary key,
	id_guest int foreign key
		references guest(id_guest),
	id_emp int foreign key
		references employees(id_emp),
	id_cus int foreign key	
		references customer(id_cus),
	id_driver int foreign key
		references driver(id_driver)
);

alter table guest_driver
add date_start datetime

alter table guest_driver
add date_end datetime

alter table guest_driver
add price float

alter table guest_driver
add [status] nvarchar(150)

alter table guest_driver
add is_delete bit

alter table guest_driver
add date_create datetime

alter table guest_driver
add date_update datetime

create table guest_car_driver(
	id_guest_car_driver int identity(1, 1) primary key,
	date_start datetime,
	date_end datetime,
	price float,
	[status] nvarchar(150), 
	is_delete bit,
	date_create datetime,
	date_update datetime,

	id_guest int foreign key
		references guest(id_guest),
	id_emp int foreign key
		references employees(id_emp),
	id_cus int foreign key	
		references customer(id_cus),
	id_driver int foreign key
		references driver(id_driver),
	id_car int foreign key
		references car(id_car)
);

create table img_car_driver(
	id_img_car_driver int identity(1, 1) primary key,
	is_delete bit,
	date_create datetime,
	date_update datetime,

	id_guest_car_driver int foreign key
	references guest_car_driver(id_guest_car_driver)
);

--------------------------------------------------------------------------------------------------------------------------------------
--PROC


--Vân

--proc xem role_task
create or alter proc sp_view_role_task
as
	begin
		select * from role_task
	end

exec sp_view_role_task

--proc xem danh sách 3 tin tức mới nhất trang chủ
create or alter proc sp_view_3_news_1
as
	begin
		select top 3 id_news, news_img, title 
		from news
		order by id_news DESC
	end

exec sp_view_3_news_1

--
create or alter proc sp_view_3_news_2
as
	begin
		select top 3 id_news, news_img, title
			from news
			where id_news not in (select top 3 id_news from news order by id_news DESC )
			order by id_news DESC 
   end

   exec sp_view_3_news_2

--
create or alter proc sp_view_3_news_3
as
	begin
		select top 3 id_news, news_img, title
			from news
			where id_news not in (select top 6 id_news from news order by id_news DESC )
			order by id_news DESC 
   end

   exec sp_view_3_news_3

--proc chuyến xe phổ biến trang chủ
create or alter proc sp_view_trip_hot_1
as
	begin
		select top 3 
			t.id_trip, [from], [to], distance, img_trip, td.price, td.time_start
			from trip t
			join trip_detail td
			on t.id_trip = td.id_trip
			where [t].[from] = N'Cần Thơ' and t.is_delete = 'False'
			order by td.id_trip DESC
   end

   exec sp_view_trip_hot_1
   
--
create or alter proc sp_view_trip_hot_2
as
	begin
		select top 3 
			t.id_trip, [from], [to], distance, img_trip, td.price, td.time_start
			from trip t
			join trip_detail td
			on t.id_trip = td.id_trip
			where [t].[from] = N'Hồ Chí Minh' and t.is_delete = 'False' and td.is_delete = 'False'
			order by td.id_trip DESC
   end

   exec sp_view_trip_hot_2

--
create or alter proc sp_view_trip_hot_3
as
	begin
		select top 3 
			t.id_trip, [from], [to], distance, img_trip, td.price, td.time_start
			from trip t
			join trip_detail td
			on t.id_trip = td.id_trip
			where [t].[from] = N'Hà Nội' and t.is_delete = 'False' and td.is_delete = 'False'
			order by td.id_trip DESC
   end

   exec sp_view_trip_hot_3

--proc khuyến mãi nổi bật trang chủ
create or alter proc sp_view_voucher_home
as
	begin
		select top 3 
			img_banner
			from banner
			where title = N'voucher' and status = 0
			order by id_banner DESC
   end

   exec sp_view_voucher_home

--proc tài xế nổi bật trong tháng trang trang chủ
create or alter proc sp_view_driver_home
as
	begin
		select top 4
			 d.id_driver, 
			 d.full_name, 
			 d.img_driver, 
			 count(td.id_driver) as trip_count, 
			 DATEDIFF(YEAR, d.birthday, GETDATE()) as birthday
			from driver d 
			join trip_detail td on d.id_driver = td.id_driver
			join guest_driver gd on d.id_driver = gd.id_driver
			join guest_car_driver gcd on d.id_driver = gd.id_driver
			where d.is_delete = 0 
					and gd.is_delete = 0 
					and gcd.is_delete = 0 
					and td.is_delete = 0
				    and gd.date_end >= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) - 1, 0)
					and gd.date_end < DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0)			
					and td.time_end >= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) - 1, 0)
					and td.time_end < DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0)			
					and gcd.date_end >= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) - 1, 0)
					and gcd.date_end < DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0)			
					group by d.id_driver, d.full_name, d.img_driver, d.birthday
			having count(td.id_driver) > 0
			order by count(td.id_driver) DESC
   end

   exec sp_view_driver_home




--------------------------------------------------------------------------------------------------------------------------------------
--Quí





--------------------------------------------------------------------------------------------------------------------------------------
--Hiếu





--------------------------------------------------------------------------------------------------------------------------------------
--Bảo





--------------------------------------------------------------------------------------------------------------------------------------
--Thịnh