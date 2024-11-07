namespace eMuhasebeServer.Domain.Abstractions
{
    //AppUser haricinde başka entity eklenirse buradaki Entity class ından inherit ederek Id alanını guid yapıp constructor yani instance üretme esnasında id nin bir guid olarak otomatik oluşması sağlandı.
    public abstract class Entity
    {
        public Guid Id { get; set; }
        protected Entity()
        {
            Id = Guid.NewGuid();
        }
    }
}
