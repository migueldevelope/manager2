using Manager.Views.Enumns;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Manager.Views.BusinessCrud
{
    public class ViewFluidCareers : _ViewCrud
    {
        public string Sphere { get; set; }
        public string Group { get; set; }
        public string Occupation { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string _idOccupation { get; set; }
        public decimal Accuracy { get; set; }
        public EnumOccupationColor Color { get; set; }
        public byte Order { get; set; }
    }
}
