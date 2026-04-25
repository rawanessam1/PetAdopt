namespace backend.Models.Enums
{
    public enum PetStatus
    {
        PendingApproval, // 3o2bal ma el admin y-approve
        Available,       
        AdoptionPending,
        Adopted,
        Rejected // lma el admin y-reject
    }
}