import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import api from "../api/axiosInstance";

const ROLES = ["Adopter", "Shelter", "PetOwner"];

export default function Register() {
  const navigate = useNavigate();
  const [form, setForm] = useState({
    name: "",
    email: "",
    password: "",
    role: "Adopter",
  });
  const [error, setError] = useState("");
  const [success, setSuccess] = useState(false);
  const [loading, setLoading] = useState(false);

  function handleChange(e) {
    setForm({ ...form, [e.target.name]: e.target.value });
  }

  async function handleSubmit(e) {
    e.preventDefault();
    setError("");
    setLoading(true);
    try {
      await api.post("/users/register", form);
      setSuccess(true);
    } catch {
      setError("Registration failed. Email may already be in use.");
    } finally {
      setLoading(false);
    }
  }

  if (success) {
    return (
      <div className="min-h-[80vh] flex items-center justify-center px-4">
        <div className="w-full max-w-md bg-surface rounded-2xl p-8 border border-white/10 text-center">
          <div className="text-5xl mb-4">🎉</div>
          <h2 className="text-2xl font-bold mb-2">Registration successful!</h2>
          {form.role === "Adopter" ? (
            <p className="text-on-surface-variant mb-6">
              You can now log in and start adopting pets.
            </p>
          ) : (
            <p className="text-on-surface-variant mb-6">
              Your account is pending admin approval before you can log in.
            </p>
          )}
          <button
            onClick={() => navigate("/login")}
            className="bg-primary text-white px-8 py-3 rounded-xl font-semibold hover:bg-primary/90 transition"
          >
            Go to Login
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-[80vh] flex items-center justify-center px-4">
      <div className="w-full max-w-md bg-surface rounded-2xl p-8 border border-white/10">
        <h1 className="text-3xl font-bold mb-2">Create account</h1>
        <p className="text-on-surface-variant mb-8">Join PetAdopt today</p>

        {error && (
          <div className="bg-red-500/10 border border-red-500/30 text-red-400 rounded-xl px-4 py-3 mb-6 text-sm">
            {error}
          </div>
        )}

        <form onSubmit={handleSubmit} className="flex flex-col gap-4">
          <div>
            <label className="block text-sm text-on-surface-variant mb-1">
              Full name
            </label>
            <input
              type="text"
              name="name"
              value={form.name}
              onChange={handleChange}
              required
              placeholder="John Doe"
              className="w-full bg-background border border-white/10 rounded-xl px-4 py-3 text-on-background focus:outline-none focus:border-primary"
            />
          </div>

          <div>
            <label className="block text-sm text-on-surface-variant mb-1">
              Email
            </label>
            <input
              type="email"
              name="email"
              value={form.email}
              onChange={handleChange}
              required
              placeholder="you@example.com"
              className="w-full bg-background border border-white/10 rounded-xl px-4 py-3 text-on-background focus:outline-none focus:border-primary"
            />
          </div>

          <div>
            <label className="block text-sm text-on-surface-variant mb-1">
              Password
            </label>
            <input
              type="password"
              name="password"
              value={form.password}
              onChange={handleChange}
              required
              minLength={8}
              placeholder="Min. 8 characters"
              className="w-full bg-background border border-white/10 rounded-xl px-4 py-3 text-on-background focus:outline-none focus:border-primary"
            />
          </div>

          <div>
            <label className="block text-sm text-on-surface-variant mb-1">
              I am a...
            </label>
            <div className="grid grid-cols-3 gap-2">
              {ROLES.map((role) => (
                <button
                  key={role}
                  type="button"
                  onClick={() => setForm({ ...form, role })}
                  className={`py-2.5 rounded-xl border text-sm font-medium transition ${
                    form.role === role
                      ? "border-primary bg-primary/10 text-primary"
                      : "border-white/10 text-on-surface-variant hover:border-white/30"
                  }`}
                >
                  {role === "Adopter"
                    ? "Adopter"
                    : role === "Shelter"
                      ? "Shelter"
                      : "Pet Owner"}
                </button>
              ))}
            </div>
            {form.role !== "Adopter" && (
              <p className="text-xs text-on-surface-variant mt-2">
                Shelter and Pet Owner accounts require admin approval before
                login.
              </p>
            )}
          </div>

          <button
            type="submit"
            disabled={loading}
            className="mt-2 w-full bg-primary text-white py-3 rounded-xl font-semibold hover:bg-primary/90 transition disabled:opacity-50"
          >
            {loading ? "Creating account..." : "Create account"}
          </button>
        </form>

        <p className="text-center text-on-surface-variant mt-6 text-sm">
          Already have an account?{" "}
          <Link to="/login" className="text-primary hover:underline">
            Sign in
          </Link>
        </p>
      </div>
    </div>
  );
}
