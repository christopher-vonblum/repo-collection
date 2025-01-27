namespace DataDomain
{
    internal class MountedRepositoryDefinition : DataEntity, IEntity
    {
        public string SourcingPath { get; set; }
        public string ActivationExpression { get; set; }
    }
}