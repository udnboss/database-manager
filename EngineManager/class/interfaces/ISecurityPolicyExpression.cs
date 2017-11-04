namespace EngineManager
{
    public interface ISecurityPolicyExpression
    {
        string ID { get; set; }
        string WhereExpression { get; }
    }
}
