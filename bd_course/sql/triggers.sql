-- Получение новой продолжительности плейлиста
CREATE OR REPLACE FUNCTION newPlaylistDuration(playlistId INT)
RETURNS interval
AS $$
    BEGIN
        IF ((
            SELECT COUNT(*)
            FROM public."SongPlaylists"
            WHERE "PlaylistId" = playlistId) > 0) THEN

            RETURN (
                SELECT SUM("Duration")
                FROM public."SongPlaylists" AS sp
                JOIN public."Songs" AS s ON sp."SongId" = s."Id"
                WHERE "PlaylistId" = playlistId
        );
        ELSE
            RETURN 0;
        END IF;
    END;

$$ LANGUAGE plpgsql


-- Обновляет продолжительности плейлиста состава после добавления или удаления песни (реализация с помощью триггеров)

CREATE OR REPLACE FUNCTION updatePlaylistDuration()
RETURNS TRIGGER
AS $$
    BEGIN
        IF (TG_OP = 'INSERT') THEN
            UPDATE public."Playlists"
            SET "Duration" = newPlaylistDuration(NEW."PlaylistId")
            WHERE "Id" = NEW."PlaylistId";

            RETURN NEW;

            ELSIF (TG_OP = 'DELETE') THEN
            UPDATE public."Playlists"
            SET "Duration" = newPlaylistDuration(OLD."PlaylistId")
            WHERE "Id" = OLD."PlaylistId";

            RETURN OLD;
        END IF;
    END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER insertDurationTrigger AFTER INSERT ON public."SongPlaylists"
    FOR EACH ROW EXECUTE PROCEDURE updatePlaylistDuration();

CREATE TRIGGER deleteDurationTrigger AFTER DELETE ON public."SongPlaylists"
    FOR EACH ROW EXECUTE PROCEDURE updatePlaylistDuration();
