import { Link } from "react-router-dom";

export default function PetCard({ pet }) {
  // Backend returns imageUrls, not images
  const firstImage = pet.imageUrls?.[0] || null;

  return (
    <Link to={`/pets/${pet.id}`} className="block group">
      <div className="bg-surface rounded-2xl overflow-hidden border border-white/10 hover:border-primary/50 hover:scale-[1.02] transition-all duration-200">
        <div className="h-48 bg-background overflow-hidden relative">
          {firstImage ? (
            <img
              src={firstImage}
              alt={pet.petName}
              className="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
            />
          ) : (
            <div className="w-full h-full flex items-center justify-center text-6xl bg-muted">
              🐾
            </div>
          )}

          {/* Status Badge */}
          {pet.status && pet.status !== "Available" && (
            <span
              className={`absolute top-3 right-3 px-3 py-1 text-xs font-medium rounded-full ${
                pet.status === "PendingApproval"
                  ? "bg-yellow-500/90 text-black"
                  : pet.status === "AdoptionPending"
                    ? "bg-orange-500/90 text-white"
                    : pet.status === "Rejected"
                      ? "bg-red-500/90 text-white"
                      : "bg-gray-500/90 text-white"
              }`}
            >
              {pet.status}
            </span>
          )}
        </div>

        <div className="p-4">
          <div className="flex items-start justify-between mb-1">
            <h3 className="font-bold text-lg">{pet.petName}</h3>
            <span className="text-xs bg-primary/10 text-primary px-2.5 py-1 rounded-full">
              {pet.type}
            </span>
          </div>

          <p className="text-on-surface-variant text-sm mb-1">{pet.breed}</p>

          {pet.ownerName && (
            <p className="text-xs text-primary/80 mb-2">🏠 {pet.ownerName}</p>
          )}

          <div className="flex items-center gap-3 text-xs text-on-surface-variant">
            <span>
              {pet.age} yr{pet.age !== 1 ? "s" : ""}
            </span>
            <span>·</span>
            <span>{pet.gender}</span>
            <span>·</span>
            <span>{pet.location}</span>
          </div>
        </div>
      </div>
    </Link>
  );
}
