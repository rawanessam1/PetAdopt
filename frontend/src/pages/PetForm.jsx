import { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { petApi } from "../api/petApi";

export default function PetForm() {
  const { id } = useParams();
  const navigate = useNavigate();
  const isEdit = Boolean(id);

  const [form, setForm] = useState({
    petName: "",
    age: "",
    breed: "",
    type: "",
    gender: "",
    description: "",
    healthStatus: "",
    location: "",
    imageUrls: [""],
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    if (isEdit) {
      petApi.getById(id).then((res) => {
        const p = res.data;
        setForm({
          petName: p.petName || "",
          age: p.age || "",
          breed: p.breed || "",
          type: p.type || "",
          gender: p.gender || "",
          description: p.description || "",
          healthStatus: p.healthStatus || "",
          location: p.location || "",
          imageUrls: p.images?.map((i) => i.url) || [""],
        });
      });
    }
  }, [id]);

  function handleChange(e) {
    setForm({ ...form, [e.target.name]: e.target.value });
  }

  function handleImageChange(index, value) {
    const updated = [...form.imageUrls];
    updated[index] = value;
    setForm({ ...form, imageUrls: updated });
  }

  function addImageField() {
    setForm({ ...form, imageUrls: [...form.imageUrls, ""] });
  }

  function removeImageField(index) {
    const updated = form.imageUrls.filter((_, i) => i !== index);
    setForm({ ...form, imageUrls: updated.length ? updated : [""] });
  }

  async function handleSubmit(e) {
    e.preventDefault();
    setError("");
    setLoading(true);
    const payload = {
      ...form,
      age: parseInt(form.age),
      imageUrls: form.imageUrls.filter((u) => u.trim() !== ""),
    };
    try {
      if (isEdit) {
        await petApi.update(id, payload);
      } else {
        await petApi.create(payload);
      }
      navigate("/my-pets");
    } catch {
      setError("Failed to save pet. Check all fields.");
    } finally {
      setLoading(false);
    }
  }

  const inputClass =
    "w-full bg-background border border-white/10 rounded-xl px-4 py-3 text-on-background focus:outline-none focus:border-primary";

  return (
    <div className="max-w-2xl mx-auto px-6 py-10">
      <h1 className="text-3xl font-bold mb-8">
        {isEdit ? "Edit Pet" : "Add New Pet"}
      </h1>

      {error && (
        <div className="bg-red-500/10 border border-red-500/30 text-red-400 rounded-xl px-4 py-3 mb-6 text-sm">
          {error}
        </div>
      )}

      <form
        onSubmit={handleSubmit}
        className="bg-surface rounded-2xl p-8 border border-white/10 flex flex-col gap-5"
      >
        <div className="grid grid-cols-2 gap-4">
          <div>
            <label className="block text-sm text-on-surface-variant mb-1">
              Pet Name
            </label>
            <input
              name="petName"
              value={form.petName}
              onChange={handleChange}
              required
              className={inputClass}
            />
          </div>
          <div>
            <label className="block text-sm text-on-surface-variant mb-1">
              Age
            </label>
            <input
              name="age"
              type="number"
              value={form.age}
              onChange={handleChange}
              required
              min="0"
              max="30"
              className={inputClass}
            />
          </div>
        </div>

        <div className="grid grid-cols-2 gap-4">
          <div>
            <label className="block text-sm text-on-surface-variant mb-1">
              Type
            </label>
            <input
              name="type"
              value={form.type}
              onChange={handleChange}
              placeholder="Dog, Cat..."
              className={inputClass}
            />
          </div>
          <div>
            <label className="block text-sm text-on-surface-variant mb-1">
              Breed
            </label>
            <input
              name="breed"
              value={form.breed}
              onChange={handleChange}
              className={inputClass}
            />
          </div>
        </div>

        <div className="grid grid-cols-2 gap-4">
          <div>
            <label className="block text-sm text-on-surface-variant mb-1">
              Gender
            </label>
            <select
              name="gender"
              value={form.gender}
              onChange={handleChange}
              className={inputClass}
            >
              <option value="">Select...</option>
              <option value="Male">Male</option>
              <option value="Female">Female</option>
            </select>
          </div>
          <div>
            <label className="block text-sm text-on-surface-variant mb-1">
              Location
            </label>
            <input
              name="location"
              value={form.location}
              onChange={handleChange}
              className={inputClass}
            />
          </div>
        </div>

        <div>
          <label className="block text-sm text-on-surface-variant mb-1">
            Health Status
          </label>
          <input
            name="healthStatus"
            value={form.healthStatus}
            onChange={handleChange}
            placeholder="Healthy, Vaccinated..."
            className={inputClass}
          />
        </div>

        <div>
          <label className="block text-sm text-on-surface-variant mb-1">
            Description
          </label>
          <textarea
            name="description"
            value={form.description}
            onChange={handleChange}
            rows={3}
            className={inputClass}
          />
        </div>

        <div>
          <label className="block text-sm text-on-surface-variant mb-2">
            Image URLs
          </label>
          {form.imageUrls.map((url, i) => (
            <div key={i} className="flex gap-2 mb-2">
              <input
                value={url}
                onChange={(e) => handleImageChange(i, e.target.value)}
                placeholder="https://..."
                className={inputClass}
              />
              <button
                type="button"
                onClick={() => removeImageField(i)}
                className="px-3 rounded-xl border border-red-500/30 text-red-400 hover:bg-red-500/10 transition"
              >
                ✕
              </button>
            </div>
          ))}
          <button
            type="button"
            onClick={addImageField}
            className="text-sm text-primary hover:underline mt-1"
          >
            + Add another image
          </button>
        </div>

        <div className="flex gap-4 pt-2">
          <button
            type="button"
            onClick={() => navigate("/my-pets")}
            className="flex-1 py-3 rounded-xl border border-white/10 text-on-surface-variant hover:border-white/30 transition"
          >
            Cancel
          </button>
          <button
            type="submit"
            disabled={loading}
            className="flex-1 bg-primary text-white py-3 rounded-xl font-semibold hover:bg-primary/90 transition disabled:opacity-50"
          >
            {loading ? "Saving..." : isEdit ? "Save Changes" : "Add Pet"}
          </button>
        </div>
      </form>
    </div>
  );
}
