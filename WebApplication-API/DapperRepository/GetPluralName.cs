namespace WebApplication_API.DapperRepository
{
    public class GetPluralName : IGetPluralName
    {
        string IGetPluralName.GetPluralName(string className)
        {
            className = className switch
            {
                "ViewName" => "MyViewName",
                _ => className + "s",
            };
            return className;
        }
    }
}
