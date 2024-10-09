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

exec sp_rename 'role_task.id_role_task', 'id', 'COLUMN'
exec sp_rename 'role_task', 'role_tasks'

create table roles(
	id_role int identity(1, 1) primary key,
	date_create datetime,
	date_update datetime
);

alter table roles
add name nvarchar(150)
exec sp_rename 'roles.id_role', 'id', 'COLUMN'


create table role_task_role(
	id_role_task_role int identity(1, 1) primary key,
	date_create datetime,
	date_update datetime,

	id_role_task int foreign key 
		references role_task(id_role_task),
	id_role int foreign key 
		references roles(id_role)
);

exec sp_rename 'role_task_role.id_role_task_role', 'id', 'COLUMN'

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

exec sp_rename 'employees.id_emp', 'id', 'COLUMN'
exec sp_rename 'employees.user_name', 'username', 'COLUMN'
exec sp_rename 'employees.pass_word', 'password', 'COLUMN'
exec sp_rename 'employees.full_name', 'fullname', 'COLUMN'


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

exec sp_rename 'customer.id_cus', 'id', 'COLUMN'
exec sp_rename 'customer.user_name', 'username', 'COLUMN'
exec sp_rename 'customer.full_name', 'fullname', 'COLUMN'
exec sp_rename 'customer', 'customers'

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

alter table customer_address
add [status] bit
exec sp_rename 'customer_address.id_address', 'id', 'COLUMN'
exec sp_rename 'customer_address', 'customer_addresses'

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

exec sp_rename 'driver.full_name', 'fullname', 'COLUMN'
exec sp_rename 'driver.id_driver', 'id', 'COLUMN'
exec sp_rename 'driver', 'driver'

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
exec sp_rename 'news.id_news', 'id', 'COLUMN'


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
exec sp_rename 'banner.id_banner', 'id', 'COLUMN'
exec sp_rename 'banner', 'banners'

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

exec sp_rename 'trip.id_trip', 'id', 'COLUMN'
exec sp_rename 'trip', 'trips'

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

alter table car
add [automatic] bit
exec sp_rename 'car.id_car', 'id', 'COLUMN'
exec sp_rename 'car', 'cars'


create table car_img(
	id_car_img int identity(1, 1) primary key,
	[name] nvarchar(max),
	is_delete bit,
	date_create datetime,
	date_update datetime,

	id_car int foreign key
		references car(id_car)
);

exec sp_rename 'car_img.id_car_img', 'id', 'COLUMN'
exec sp_rename 'car_img', 'car_imgs'
exec sp_rename 'car_imgs.id_car', 'car_id'


create table car_seat(
	id_car_seat int identity(1, 1) primary key,
	[name] nvarchar(50),
	is_delete bit,
	date_create datetime,
	date_update datetime,

	id_car int foreign key 
		references car(id_car)
);

exec sp_rename 'car_seat.id_car_seat', 'id', 'COLUMN'
exec sp_rename 'car_seat', 'car_seats'
exec sp_rename 'car_seats.id_car', 'car_id', 'COLUMN'


create table [location](
	id_location int identity(1, 1) primary key,
	[name] nvarchar(500),
	[address] nvarchar(500),
	phone_number nvarchar(20),
	date_create datetime,
	date_update datetime,
	is_delete bit
);

exec sp_rename 'location.id_location', 'id', 'COLUMN'
exec sp_rename 'location', 'locations'

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

alter table trip_detail 
add location_to_id int

exec sp_rename 'trip_detail.id_trip_detail', 'id', 'COLUMN'
exec sp_rename 'trip_detail.id_location', 'location_from_id', 'COLUMN'
exec sp_rename 'trip_detail', 'trip_details'
exec sp_rename 'trip_details.id_trip', 'trip_id', 'COLUMN'
exec sp_rename 'trip_details.id_car', 'car_id', 'COLUMN'
exec sp_rename 'trip_details.id_driver', 'driver_id', 'COLUMN'

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

alter table guest_car
alter column [status] int
exec sp_rename 'guest_car.id_guest_car', 'id', 'COLUMN'
exec sp_rename 'guest_car', 'guest_cars'
exec sp_rename 'guest_cars.id_emp', 'emp_id', 'COLUMN'
exec sp_rename 'guest_cars.id_car', 'car_id', 'COLUMN'
exec sp_rename 'guest_cars.id_cus', 'cus_id', 'COLUMN'


create table img_guest_car(
	id_img_guest_car int identity(1, 1) primary key,
	is_delete bit,
	date_create datetime,
	date_update datetime,

	id_guest_car int foreign key
		references guest_car(id_guest_car)
);

exec sp_rename 'img_guest_car.id_img_guest_car', 'id', 'COLUMN'
exec sp_rename 'img_guest_car', 'img_guest_cars'
exec sp_rename 'img_guest_cars.id_guest_car', 'guest_car_id', 'COLUMN'

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

alter table guest_trip
alter column [status] int
exec sp_rename 'guest_trip', 'guests_trip'
exec sp_rename 'guests_trip.id_guest_trip', 'id', 'COLUMN'
exec sp_rename 'guests_trip.id_emp', 'emp_id', 'COLUMN'
exec sp_rename 'guests_trip.id_cus', 'cus_id', 'COLUMN'
exec sp_rename 'guests_trip.id_trip_detail', 'trip_detail_id', 'COLUMN'


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

alter table seat_guest_trip
alter column [status] int
exec sp_rename 'seat_guest_trip.id_seat_guest_trip', 'id', 'COLUMN'
exec sp_rename 'seat_guest_trip.id_guest_trip', 'guest_trip_id', 'COLUMN'
exec sp_rename 'seat_guest_trip.id_car_seat', 'car_seat_id', 'COLUMN'

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

exec sp_rename 'guest_driver.id_guest_driver', 'id', 'COLUMN'
exec sp_rename 'guest_driver.id_emp', 'emp_id', 'COLUMN'
exec sp_rename 'guest_driver.id_cus', 'cus_id', 'COLUMN'
exec sp_rename 'guest_driver.id_driver', 'driver_id', 'COLUMN'


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

exec sp_rename 'guest_car_driver.id_guest_car_driver', 'id', 'COLUMN'
exec sp_rename 'guest_car_driver.id_emp', 'emp_id', 'COLUMN'
exec sp_rename 'guest_car_driver.id_cus', 'cus_id', 'COLUMN'
exec sp_rename 'guest_car_driver.id_driver', 'driver_id', 'COLUMN'
exec sp_rename 'guest_car_driver.id_car', 'car-id', 'COLUMN'

create table img_car_driver(
	id_img_car_driver int identity(1, 1) primary key,
	is_delete bit,
	date_create datetime,
	date_update datetime,

	id_guest_car_driver int foreign key
	references guest_car_driver(id_guest_car_driver)
);

exec sp_rename 'img_car_driver.id_img_car_driver', 'id', 'COLUMN'
exec sp_rename 'img_car_driver.id_guest_car_driver', 'guest_car_driver_id', 'COLUMN'

alter table guest_trip
drop constraint FK__guest_tri__id_gu__6D0D32F4
alter table guest_driver
drop constraint FK__guest_dri__id_gu__72C60C4A
alter table guest_car_driver
drop constraint FK__guest_car__id_gu__787EE5A0
alter table guest
drop constraint FK__guest__id_emp__47DBAE45
alter table guest_car
drop constraint FK__guest_car__id_gu__6477ECF3

alter table guest_trip
drop column id_guest
alter table guest_driver
drop column id_guest
alter table guest_car_driver
drop column id_guest
alter table guest_car
drop column id_guest

drop table guest

alter table guest_trip
add foreign key(id_trip_detail) references trip_detail(id_trip_detail)

alter table guest_driver
add foreign key(id_driver) references driver(id_driver)

exec sp_rename 'customers.id_role', 'role_id', 'COLUMN'
exec sp_rename 'customers.pass_word', 'password', 'COLUMN'

ALTER TABLE dbo.cars
ADD car_code VARCHAR(20)
ALTER TABLE dbo.cars
ADD price float
ALTER TABLE dbo.cars
ADD voucher float
ALTER TABLE dbo.cars
ADD status BIT

--------------------------------------------------------------------------------------------------------------------------------------
--PROC


--Vân

--proc xem role_task
create or alter proc sp_view_role_task
as
	begin
		select id, name from role_tasks
		--where isDelete = 'False'
		order by id desc
	end

exec sp_view_role_task

--proc xem role_task
create or alter proc sp_getid_role_task
	@id int
as
	begin
		select id, name from role_tasks
		where id = @id
			--and isDelete = 'False'
		order by id desc
	end

exec sp_getid_role_task 18


--proc them role_task
create or alter proc sp_add_role_task
	@name nvarchar(200)
as
	begin
		insert into role_tasks
		(
		name,
		date_create
		)
	values
		(
		@name,
		GETDATE()
		)
	end

exec sp_add_role_task 'Van demo role task'

--proc sua role_task 
create or alter proc sp_update_role_task
	@name nvarchar(200),
	@id int
as
	begin
	update role_tasks
	set
		name = @name,
		date_update = GETDATE()
	where
		id = @id
	end

exec sp_update_role_task 'Van demo role task', 18

--proc xem danh sách 3 tin tức mới nhất trang chủ
create or alter proc sp_view_3_news_1
as
	begin
		select top 3 id, news_img, title 
		from news
		order by id DESC
	end

exec sp_view_3_news_1

--proc xem danh sách 3 tin tức mới nhất trang chủ
create or alter proc sp_view_3_news_1
as
	begin
		select top 3 id, news_img, title 
		from news
		order by id DESC
	end

exec sp_view_3_news_1

--
create or alter proc sp_view_3_news_2
as
	begin
		select top 3 id, news_img, title
			from news
			where id not in (select top 3 id from news order by id DESC )
			order by id DESC 
   end

   exec sp_view_3_news_2

--
create or alter proc sp_view_3_news_3
as
	begin
		select top 3 id, news_img, title
			from news
			where id not in (select top 6 id from news order by id DESC )
			order by id DESC 
   end

   exec sp_view_3_news_3

--proc chuyến xe phổ biến trang chủ
create or alter proc sp_view_trip_hot_1
as
	begin
		select top 3 
			t.id, [from], [to], distance, img_trip, td.price, td.time_start
			from trips t
			join trip_details td
			on t.id = td.trip_id
			where [t].[from] = N'Cần Thơ' and t.is_delete = 'False'
			order by td.trip_id DESC
   end

   exec sp_view_trip_hot_1
   
--
create or alter proc sp_view_trip_hot_2
as
	begin
		select top 3 
			t.id, [from], [to], distance, img_trip, td.price, td.time_start
			from trips t
			join trip_details td
			on t.id = td.trip_id
			where [t].[from] = N'Hồ Chí Minh' and t.is_delete = 'False' and td.is_delete = 'False'
			order by td.trip_id DESC
   end

   exec sp_view_trip_hot_2

--
create or alter proc sp_view_trip_hot_3
as
	begin
		select top 3 
			t.id, [from], [to], distance, img_trip, td.price, td.time_start
			from trips t
			join trip_details td
			on t.id = td.trip_id
			where [t].[from] = N'Hà Nội' and t.is_delete = 'False' and td.is_delete = 'False'
			order by td.trip_id DESC
   end

   exec sp_view_trip_hot_3

--proc khuyến mãi nổi bật trang chủ
create or alter proc sp_view_voucher_home
as
	begin
		select top 3 
			img_banner
			from banners
			where title = N'voucher' and status = 0
			order by id DESC
   end

   exec sp_view_voucher_home

--proc tài xế nổi bật trong tháng trang trang chủ
create or alter proc sp_view_driver_home
as
	begin
		select top 4
			 d.id, 
			 d.fullname, 
			 d.img_driver, 
			 count(td.driver_id) as trip_count, 
			 DATEDIFF(YEAR, d.birthday, GETDATE()) as birthday
			from driver d 
			join trip_details td on d.id = td.driver_id
			join guest_driver gd on d.id = gd.driver_id
			join guest_car_driver gcd on d.id = gd.driver_id
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
					group by d.id, d.fullname, d.img_driver, d.birthday
			having count(td.driver_id) > 0
			order by count(td.driver_id) DESC
   end

   exec sp_view_driver_home

--proc đăng nhập user
create or alter proc sp_user_login
	@username varchar(50),
	@password varchar(50)
	as
	begin
		select *
			from customers 
			where customers.is_delete = 0 
				and @username = username
				and @password = [customers].[password]
		end

   exec sp_user_login haivan55, haivan

 --proc đăng nhập admin
create or alter proc sp_admin_login
	@email varchar(50),
	@password varchar(50)
	as
	begin
		select *
			from dbo.employees 
			where is_delete = 0 
				and @email = email
				and @password = password
		end

   exec sp_admin_login 'admin', 'admin123'

   SELECT* FROM dbo.cars
--------------------------------------------------------------------------------------------------------------------------------------
--Quí





--------------------------------------------------------------------------------------------------------------------------------------
--Hiếu





--------------------------------------------------------------------------------------------------------------------------------------
--Bảo





--------------------------------------------------------------------------------------------------------------------------------------
--Thịnh