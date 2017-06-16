namespace BaoZhong.Model
{
    public abstract class BaseModel
    {
        protected string ImageServerUrl = "";

        public object Id
        {
            get;
            set;
        }
    }
}
