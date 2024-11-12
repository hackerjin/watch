using Lephone.Data.Definition;

namespace Skyray.EDXRFLibrary
{
    [Cacheable]
    public abstract class Role : DbObjectModel<Role>
    {
        [Length(ColLength.RoleName)]
        public abstract string RoleName { get; set; }

        public abstract int RoleType { set; get; }

        [HasMany(OrderBy = "Id")]
        public abstract HasMany<User> Users { get; set; }

        public abstract Role Init(string RoleName, int RoleType);
    }
}