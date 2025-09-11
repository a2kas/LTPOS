namespace POS_display.Repository
{
    public abstract class BaseRepository
    {
       public DB_Base DB_Base => new DB_Base();
    }
}
