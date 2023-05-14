namespace ApartmentRentManagementSystem.Dtos
{
    public class HouseEquipmentDto
    {
        public string Name{get; set;}
        public string Description{get; set;}
    }
    public class HouseEquipmentRequestModel
    {
        public string Name{get; set;}
        public string Description{get; set;}
        public int ApartmentId{get; set;}
    }
}