create table users (
id_user int primary key identity (1,1),
fio nvarchar (50) not null,
phone nvarchar (11)not null,
login nvarchar (50)not null,
password nvarchar (50)not null,
user_type int not null,
FOREIGN  key (user_type) references user_type (id_user_type))
