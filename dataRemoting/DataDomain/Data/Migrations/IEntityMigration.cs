using System.Linq.Expressions;

namespace DataDomain.Migrations
{
    public interface IEntityMigration : IEntity
    {
        IClrTypeMigrationIdentity SourceType { get; set; }
        IClrTypeMigrationIdentity TargetType { get; set; }
        Expression<EntityMigrationDelegate> UpgradeDelegate { get; set; }
        Expression<EntityMigrationDelegate> DowngradeDelegate { get; set; }
    }
}