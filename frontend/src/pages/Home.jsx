import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import api from "../api/axiosInstance";
import PetCard from "../components/PetCard";

export default function Home() {
  const { user } = useAuth();

  const [pets, setPets] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    fetchPets();
  }, [user]); // ← Re-fetch when user logs in / changes role

  async function fetchPets() {
    setLoading(true);
    try {
      const res = await api.get("/pets");
      setPets(res.data);
    } catch (err) {
      console.error(err);
      setError("Failed to load pets.");
      setPets([]);
    } finally {
      setLoading(false);
    }
  }

  return (
    <div>
      {/* Hero Section */}
      <div className="text-center px-4 py-20 bg-gradient-to-b from-primary/10 to-transparent">
        <h1 className="text-5xl font-bold mb-4">Find your perfect companion</h1>
        <p className="text-on-surface-variant text-xl mb-8 max-w-2xl mx-auto">
          Thousands of pets are waiting for a loving home
        </p>
        <Link
          to="/browse"
          className="inline-block bg-primary text-white px-8 py-3 rounded-full font-semibold hover:bg-primary/90 transition"
        >
          Browse all pets
        </Link>
      </div>

      {/* Recently Added / Featured Pets */}
      <div className="max-w-7xl mx-auto px-6 pb-20">
        <div className="flex items-center justify-between mb-6">
          <h2 className="text-2xl font-bold">
            {user?.role === "Admin" ||
            user?.role === "Shelter" ||
            user?.role === "PetOwner"
              ? "All Pets"
              : "Recently Added"}
          </h2>

          <Link
            to="/browse"
            className="text-primary hover:text-primary/80 text-sm font-medium flex items-center gap-1"
          >
            View all →
          </Link>
        </div>

        {error && (
          <div className="bg-red-500/10 border border-red-500/30 text-red-400 rounded-xl px-4 py-3 mb-6">
            {error}
          </div>
        )}

        {loading ? (
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
            {[...Array(8)].map((_, i) => (
              <div
                key={i}
                className="bg-surface rounded-2xl h-72 animate-pulse"
              />
            ))}
          </div>
        ) : pets.length === 0 ? (
          <div className="text-center py-20 text-on-surface-variant">
            No pets available right now.
          </div>
        ) : (
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
            {pets.map((pet) => (
              <PetCard key={pet.id} pet={pet} />
            ))}
          </div>
        )}
      </div>
    </div>
  );
}
