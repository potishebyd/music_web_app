-- роль Администратора

CREATE ROLE admin WITH
    SUPERUSER
    NOCREATEDB
    CREATEROLE
    NOINHERIT
    NOREPLICATION
    NOBYPASSRLS
    CONNECTION LIMIT -1
    LOGIN
    PASSWORD 'admin';

-- права доступа

GRANT ALL PRIVILEGES 
    ON ALL TABLES IN SCHEMA public 
    TO admin;

-- роль Авторизированного пользователя

CREATE ROLE _user WITH
    NOSUPERUSER
    NOCREATEDB
    NOCREATEROLE
    NOINHERIT
    NOREPLICATION
    NOBYPASSRLS
    CONNECTION LIMIT -1
    LOGIN
    PASSWORD 'user';

-- права доступа

GRANT SELECT
    ON ALL TABLES IN SCHEMA public 
    TO _user;

GRANT INSERT
    ON public."Playlists", 
       public."SongPlaylists", 
       public."Users"
    TO _user;

GRANT DELETE
    ON public."SongPlaylists"
    TO _user;

GRANT UPDATE
    ON public."Playlists" 
    TO _user;

-- роль Гостя

CREATE ROLE guest WITH
    NOSUPERUSER
    NOCREATEDB
    NOCREATEROLE
    NOINHERIT
    NOREPLICATION
    NOBYPASSRLS
    CONNECTION LIMIT -1
    LOGIN
    PASSWORD 'guest';

-- права доступа

GRANT SELECT
    ON public."Artists", 
       public."RecordingStudios", 
       public."Songs", 
       public."Playlists", 
       public."Users"
    TO guest;

GRANT INSERT
    ON public."Playlists", 
       public."Users"
    TO guest;

-- Удалить права доступа

-- REVOKE SELECT 
--     ON public."Club" 
--     TO _user;

-- Удалить роль

-- DROP ROLE guest;
