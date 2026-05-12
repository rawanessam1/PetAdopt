import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { petApi } from "../api/petApi";
import { favoriteApi } from "../api/favoriteApi";
import { requestApi } from "../api/requestApi";

export default function PetDetails() {
  const { id } = useParams();
  const { user } = useAuth();
  const navigate = useNavigate();

  const [pet, setPet] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [requestSent, setRequestSent] = useState(false);
  const [favorited, setFavorited] = useState(false);
  const [actionMsg, setActionMsg] = useState("");

  // For Adopters
  const [history, setHistory] = useState([]);
  const [selectedHistory, setSelectedHistory] = useState([]);
  const [extraNotes, setExtraNotes] = useState("");

  // Fetch pet details
  useEffect(() => {
    const fetchPet = async () => {
      try {
        const res = await petApi.getById(id);
        setPet(res.data);
      } catch (err) {
        setError("Pet not found or unavailable.");
      } finally {
        setLoading(false);
      }
    };

    fetchPet();
  }, [id]);

  // Check favorite + fetch history
  useEffect(() => {
    if (!user || !pet?.id) return;

    favoriteApi
      .getMyFavorites()
      .then((res) => {
        const isFav = res.data?.some((f) => f.petId === parseInt(id)) || false;
        setFavorited(isFav);
      })
      .catch(() => setFavorited(false));

    if (user.role === "Adopter") {
      requestApi
        .getHistory()
        .then((res) => setHistory(res.data || []))
        .catch(() => setHistory([]));
    }
  }, [user, pet?.id, id]);

  const toggleHistory = (petName) => {
    setSelectedHistory((prev) =>
      prev.includes(petName)
        ? prev.filter((p) => p !== petName)
        : [...prev, petName],
    );
  };

  const buildAdoptionHistory = () => {
    const parts = [];
    if (selectedHistory.length > 0) {
      parts.push(`Previously adopted: ${selectedHistory.join(", ")}`);
    }
    if (extraNotes.trim()) parts.push(extraNotes.trim());
    return parts.join(" | ");
  };

  const handleAdopt = async () => {
    if (!user) return navigate("/login");

    try {
      await requestApi.send({
        petId: parseInt(id),
        adoptionHistory: buildAdoptionHistory(),
      });
      setRequestSent(true);
      setActionMsg("Adoption request sent successfully! 🎉");
    } catch (err) {
      setActionMsg("You may have already applied for this pet.");
    }
  };

  const handleFavorite = async () => {
    if (!user) return navigate("/login");

    try {
      if (favorited) {
        await favoriteApi.remove(parseInt(id));
        setFavorited(false);
      } else {
        await favoriteApi.add(parseInt(id));
        setFavorited(true);
      }
    } catch {
      setActionMsg("Failed to update favorite.");
    }
  };

  if (loading) {
    return (
      <div className="min-h-[60vh] flex items-center justify-center">
        <p className="text-xl text-on-surface-variant">
          Loading pet details...
        </p>
      </div>
    );
  }

  if (error || !pet) {
    return (
      <div className="min-h-[60vh] flex items-center justify-center">
        <p className="text-red-400 text-xl">{error || "Pet not found"}</p>
      </div>
    );
  }

  const isOwner = user?.id === pet.ownerId;
  const canAdopt =
    user?.role === "Adopter" && pet.status === "Available" && !isOwner;

  return (
    <div className="max-w-4xl mx-auto px-6 py-10">
      <button
        onClick={() => navigate(-1)}
        className="text-on-surface-variant hover:text-primary mb-6 flex items-center gap-1"
      >
        ← Back
      </button>

      {/* Image */}
      <div className="aspect-video bg-muted rounded-3xl overflow-hidden mb-8 shadow-lg">
        {pet.imageUrls?.length > 0 ? (
          <img
            src={pet.imageUrls[0]}
            alt={pet.petName}
            className="w-full h-full object-cover"
          />
        ) : (
          <div className="w-full h-full flex items-center justify-center text-8xl bg-background">
            🐾
          </div>
        )}
      </div>

      <div className="p-8 bg-background rounded-3xl">
        <div className="flex flex-col md:flex-row md:items-start justify-between gap-4 mb-6">
          <div>
            <h1 className="text-4xl font-bold mb-1">{pet.petName}</h1>
            <p className="text-on-surface-variant text-lg">
              {pet.breed} · {pet.type}
            </p>
            {pet.ownerName && (
              <p className="text-sm text-primary/80 mt-2 flex items-center gap-1">
                🏠 Owner: <span className="font-medium">{pet.ownerName}</span>
              </p>
            )}
          </div>

          <span
            className={`px-5 py-2 rounded-full text-sm font-semibold self-start ${
              pet.status === "Available"
                ? "bg-green-500/10 text-green-500"
                : pet.status === "Adopted"
                  ? "bg-gray-500/10 text-gray-400"
                  : "bg-yellow-500/10 text-yellow-500"
            }`}
          >
            {pet.status}
          </span>
        </div>

        {/* Status Message for Adopters */}
        {user?.role === "Adopter" && pet.status !== "Available" && (
          <div className="bg-yellow-500/10 border border-yellow-500/30 text-yellow-600 dark:text-yellow-400 p-4 rounded-2xl mb-8">
            This pet is currently <strong>not available</strong> for adoption.
          </div>
        )}

        {/* Details Grid */}
        <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mb-10">
          {[
            {
              label: "Age",
              value: `${pet.age} year${pet.age !== 1 ? "s" : ""}`,
            },
            { label: "Gender", value: pet.gender },
            { label: "Location", value: pet.location },
            { label: "Health", value: pet.healthStatus },
          ].map((item) => (
            <div key={item.label} className="bg-card rounded-2xl p-5">
              <p className="text-xs text-on-surface-variant mb-1">
                {item.label}
              </p>
              <p className="font-semibold text-lg">{item.value}</p>
            </div>
          ))}
        </div>

        {/* Description */}
        {pet.description && (
          <div className="mb-10">
            <h2 className="text-xl font-semibold mb-3">About {pet.petName}</h2>
            <p className="text-on-surface-variant leading-relaxed whitespace-pre-line">
              {pet.description}
            </p>
          </div>
        )}

        {actionMsg && (
          <div className="bg-primary/10 border border-primary/30 text-primary rounded-2xl px-5 py-4 mb-6">
            {actionMsg}
          </div>
        )}

        {/* Adoption Section */}
        {canAdopt && (
          <div className="space-y-6">
            {history.length > 0 && (
              <div className="bg-background rounded-xl p-4 border border-white/10">
                <p className="text-sm font-semibold mb-3">
                  📋 Your previous adoptions
                  <span className="text-xs text-on-surface-variant ml-2 font-normal">
                    (automatically included in your request)
                  </span>
                </p>
                <div className="flex flex-wrap gap-2">
                  {history.map((h) => (
                    <span
                      key={h.id}
                      className="px-3 py-1.5 rounded-full text-sm border border-primary/30 bg-primary/10 text-primary"
                    >
                      ✓ {h.petName}
                    </span>
                  ))}
                </div>
              </div>
            )}

            <div>
              <label className="block text-sm text-on-surface-variant mb-2">
                Additional notes
              </label>
              <textarea
                value={extraNotes}
                onChange={(e) => setExtraNotes(e.target.value)}
                disabled={requestSent}
                rows={4}
                placeholder="Tell us about your experience, home setup, or why you'd be a great match..."
                className="w-full bg-card border border-white/10 rounded-2xl px-5 py-4 focus:border-primary resize-none disabled:opacity-60"
              />
            </div>

            <div className="flex gap-4 pt-4">
              <button
                onClick={handleAdopt}
                disabled={requestSent}
                className="flex-1 bg-primary hover:bg-primary/90 text-white py-4 rounded-2xl font-semibold text-lg transition disabled:opacity-50"
              >
                {requestSent ? "Request Sent ✓" : "Request Adoption"}
              </button>

              <button
                onClick={handleFavorite}
                className={`px-8 py-4 rounded-2xl border font-semibold transition ${
                  favorited
                    ? "bg-red-500/10 border-red-500 text-red-400"
                    : "border-white/10 hover:border-white/30"
                }`}
              >
                {favorited ? "❤️ Saved" : "🤍 Save"}
              </button>
            </div>
          </div>
        )}

        {!user && (
          <button
            onClick={() => navigate("/login")}
            className="w-full bg-primary text-white py-4 rounded-2xl font-semibold text-lg"
          >
            Login to Request Adoption
          </button>
        )}

        {isOwner && (
          <p className="text-center text-on-surface-variant py-6">
            This is your listed pet
          </p>
        )}
      </div>
    </div>
  );
}
