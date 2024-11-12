using Lephone.Data.Definition;

namespace Skyray.EDXRFLibrary
{
    [Cacheable]
    public abstract class User : DbObjectModel<User>
    {
        [Length(ColLength.UserName)]
        public abstract string Name { get; set; }

        [Length(ColLength.Password)]
        public abstract string Password { get; set; }

        [LazyLoad, Length(ColLength.RePassword)]
        public abstract string RePassword { get; set; }

        [BelongsTo, DbColumn("Role_Id")]
        public abstract Role Role { get; set; }

        public abstract User Init(string name, string password, string RePassword);
    }
}
