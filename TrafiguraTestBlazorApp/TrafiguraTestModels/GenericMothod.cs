namespace TrafiguraTestModels;

public static class GenericMothod
{

    public static string GetErrorMessage(int code)
    {
        switch (code)
        {
            case 200:
                return "Operation Success!!!";
            case 500:
                return "Internal server Error!!!";
            case 590:
                return "No Data Found!!!";
            case 591:
                return "Invalid Data!!!";
            default:
                return $"Error - {code}, Server Error!!!";
        }    
    }
}
