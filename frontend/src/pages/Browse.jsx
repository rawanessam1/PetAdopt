import { useState, useEffect } from "react";
import { useAuth } from "../context/AuthContext";
import api from "../api/axiosInstance";
import PetCard from "../components/PetCard";

export default function Browse() {
  const { user } = useAuth();

  const [pets, setPets] = useState([]);
  const [loading, setLoading] = useState(false);
  const [filters, setFilters] = useState({
    type: "",
    breed: "",
    age: "",
    location: "",
  });

  // Fetch pets on mount and when user changes (important for role-based visibility)
  useEffect(() => {
    fetchPets();
  }, [user]);

  async function fetchPets() {
    setLoading(true);
    try {
      const res = await api.get("/pets");
      setPets(res.data);
    } catch (err) {
      console.error(err);
      setPets([]);
    } finally {
      setLoading(false);
    }
  }

  async function search(e) {
    if (e) e.preventDefault();

    setLoading(true);
    try {
      const params = {};
      if (filters.type) params.type = filters.type;
      if (filters.breed) params.breed = filters.breed;
      if (filters.age) params.age = filters.age;
      if (filters.location) params.location = filters.location;

      const res = await api.get("/pets/search", { params });
      setPets(res.data);
    } catch {
      setPets([]);
    } finally {
      setLoading(false);
    }
  }

  function handleChange(e) {
    setFilters({ ...filters, [e.target.name]: e.target.value });
  }

  return (
    <div className="max-w-7xl mx-auto px-6 py-10">
      <h1 className="text-3xl font-bold mb-8">Browse Pets</h1>

      {/* Filters */}
      <form
        onSubmit={search}
        className="bg-surface rounded-2xl p-6 border border-white/10 mb-8"
      >
        <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
          <div>
            <label className="block text-sm text-on-surface-variant mb-1">
              Type
            </label>
            <input
              name="type"
              value={filters.type}
              onChange={handleChange}
              placeholder="Dog, Cat..."
              className="w-full bg-background border border-white/10 rounded-xl px-4 py-2.5 text-sm focus:outline-none focus:border-primary"
            />
          </div>
          <div>
            <label className="block text-sm text-on-surface-variant mb-1">
              Breed
            </label>
            <input
              name="breed"
              value={filters.breed}
              onChange={handleChange}
              placeholder="Golden Retriever..."
              className="w-full bg-background border border-white/10 rounded-xl px-4 py-2.5 text-sm focus:outline-none focus:border-primary"
            />
          </div>
          <div>
            <label className="block text-sm text-on-surface-variant mb-1">
              Age
            </label>
            <input
              name="age"
              type="number"
              value={filters.age}
              onChange={handleChange}
              placeholder="e.g. 2"
              min="0"
              className="w-full bg-background border border-white/10 rounded-xl px-4 py-2.5 text-sm focus:outline-none focus:border-primary"
            />
          </div>
          <div>
            <label className="block text-sm text-on-surface-variant mb-1">
              Location
            </label>
            <input
              name="location"
              value={filters.location}
              onChange={handleChange}
              placeholder="e.g. Cairo"
              className="w-full bg-background border border-white/10 rounded-xl px-4 py-2.5 text-sm focus:outline-none focus:border-primary"
            />
          </div>
        </div>

        <div className="flex gap-3 mt-4">
          <button
            type="submit"
            className="bg-primary text-white px-8 py-2.5 rounded-xl font-medium hover:bg-primary/90 transition"
          >
            Search
          </button>
          <button
            type="button"
            onClick={() => {
              setFilters({ type: "", breed: "", age: "", location: "" });
              fetchPets();
            }}
            className="border border-white/20 px-6 py-2.5 rounded-xl hover:bg-white/5 transition"
          >
            Clear
          </button>
        </div>
      </form>

      {/* Results */}
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
          No pets found.
        </div>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
          {pets.map((pet) => (
            <PetCard key={pet.id} pet={pet} />
          ))}
        </div>
      )}
    </div>
  );
}
