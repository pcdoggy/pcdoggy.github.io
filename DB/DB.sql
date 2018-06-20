-- Table: s_code

-- DROP TABLE s_code;

CREATE TABLE s_code (
  sub1 smallint NOT NULL,
  sub2 smallint NOT NULL,
  code varchar(6) NOT NULL,
  name nvarchar(20) NOT NULL,
  notes nvarchar(100),
  useflag bit NOT NULL,
  orderid int NOT NULL,
  CONSTRAINT s_code_pkey PRIMARY KEY (sub1 , sub2 , code )
)
-- Index: s_code_idx0

-- DROP INDEX s_code_idx0;

CREATE  INDEX s_code_idx0
 ON s_code
(sub1 , sub2 );
-- Table: s_serial

-- DROP TABLE s_serial;

CREATE TABLE s_serial (
  character1 varchar(6) NOT NULL,
  character2 varchar(14) NOT NULL,
  curnumber int NOT NULL,
  CONSTRAINT s_serial_pkey PRIMARY KEY (character1 , character2 )
)
-- Table: s_user

-- DROP TABLE s_user;

CREATE TABLE s_user (
  uid int NOT NULL,
  userno varchar(20) NOT NULL,
  nickname nvarchar(20),
  username nvarchar(20) NOT NULL,
  useremail varchar(50) NOT NULL,
  usertype varchar(6) NOT NULL,
  telno varchar(20),
  userpass varchar(64) NOT NULL,
  validcode varchar(10),
  validtime datetime,
  token varchar(32),
  tokentime datetime,
  instime datetime NOT NULL,
  updtime datetime NOT NULL,
  CONSTRAINT s_user_pkey PRIMARY KEY (uid )
)
-- Index: s_user_idx0

-- DROP INDEX s_user_idx0;

CREATE  INDEX s_user_idx0
 ON s_user
(uid );
-- Index: s_user_idx1

-- DROP INDEX s_user_idx1;

CREATE  INDEX s_user_idx1
 ON s_user
(userno );
-- Index: s_user_idx2

-- DROP INDEX s_user_idx2;

CREATE  INDEX s_user_idx2
 ON s_user
(username );
-- Index: s_user_idx3

-- DROP INDEX s_user_idx3;

CREATE  INDEX s_user_idx3
 ON s_user
(useremail );
-- Table: s_interface

-- DROP TABLE s_interface;

CREATE TABLE s_interface (
  iid int NOT NULL,
  userno varchar(20) NOT NULL,
  intertype varchar(20) NOT NULL,
  token varchar(32),
  clientsystime varchar(17),
  CONSTRAINT s_interface_pkey PRIMARY KEY (iid )
)
-- Index: s_interface_idx0

-- DROP INDEX s_interface_idx0;

CREATE  INDEX s_interface_idx0
 ON s_interface
(userno , intertype );
-- Table: ebs_book

-- DROP TABLE ebs_book;

CREATE TABLE ebs_book (
  bid bigint NOT NULL,
  bno varchar(20) NOT NULL,
  btitle nvarchar(100) NOT NULL,
  burl nvarchar(300) NOT NULL,
  author varchar(17) NOT NULL,
  briefintro nvarchar(1000) NOT NULL,
  btype varchar(6) NOT NULL,
  bstatus varchar(6) NOT NULL,
  chapcount int NOT NULL,
  bcover nvarchar(300) NOT NULL,
  insuser varchar(20) NOT NULL,
  instime datetime NOT NULL,
  upduser varchar(20) NOT NULL,
  updtime datetime NOT NULL,
  CONSTRAINT ebs_book_pkey PRIMARY KEY (bid )
)
-- Index: ebs_book_idx0

-- DROP INDEX ebs_book_idx0;

CREATE  INDEX ebs_book_idx0
 ON ebs_book
(bno );
-- Index: ebs_book_idx1

-- DROP INDEX ebs_book_idx1;

CREATE  INDEX ebs_book_idx1
 ON ebs_book
(btitle );
-- Table: ebs_book_content

-- DROP TABLE ebs_book_content;

CREATE TABLE ebs_book_content (
  conno varchar(20) NOT NULL,
  bno varchar(20) NOT NULL,
  chapindex int NOT NULL,
  chapname nvarchar(100) NOT NULL,
  chapcontent nvarchar(max) NOT NULL,
  chapurl nvarchar(300) NOT NULL,
  insuser varchar(20) NOT NULL,
  instime datetime NOT NULL,
  upduser varchar(20) NOT NULL,
  updtime datetime NOT NULL,
  CONSTRAINT ebs_book_content_pkey PRIMARY KEY (conno , bno )
)
-- Index: ebs_book_content_idx0

-- DROP INDEX ebs_book_content_idx0;

CREATE  INDEX ebs_book_content_idx0
 ON ebs_book_content
(conno , bno , chapindex );

