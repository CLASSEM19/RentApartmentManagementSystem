namespace ApartmentRentManagementSystem.Dtos
{
    public class UtilityDto
    {
        public string Name{get; set;}
        public int Quantity{get; set;}
        public string Description{get; set;}
    }
    public class UtilityRequestModel
    {
        public string Name{get; set;}
        public int Quantity{get; set;}
        public string Description{get; set;}
        public int ApartmentId{get; set;}
    }
}