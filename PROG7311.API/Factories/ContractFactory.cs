namespace PROG7311_POE.Factories
{

    public class ContractFactory
    {
        public static IContract GetContract(string type)
        {
            return type switch
            {
                "Premium" => new PremiumContract(),
                _ => new StandardContract()
            };
        }
    }
}
