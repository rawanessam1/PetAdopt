import { useEffect, useState } from "react";
import { authApi } from "../api/authApi";
import { petApi } from "../api/petApi";
import api from "../api/axiosInstance";

export default function Admin() {
  const [users, setUsers] = useState([]);
  const [pets, setPets] = useState([]);
  const [tab, setTab] = useState("users");
  const [msg, setMsg] = useState("");

  useEffect(() => {
    api
      .get("/users/pending")
      .then((res) => setUsers(res.data))
      .catch(() => setUsers([]));
    petApi
      .getPending()
      .then((res) => setPets(res.data))
      .catch(() => setPets([]));
  }, []);
  async function handleUser(id, action) {
    try {
      if (action === "approve") await authApi.approveUser(id);
      else await authApi.rejectUser(id);
      setUsers((prev) => prev.filter((u) => u.id !== id));
      setMsg(`User ${action}d.`);
    } catch {
      setMsg("Action failed.");
    }
  }

  async function handlePet(id, action) {
    try {
      if (action === "approve") await petApi.approve(id);
      else await petApi.reject(id);
      setPets((prev) => prev.filter((p) => p.id !== id));
      setMsg(`Pet ${action}d.`);
    } catch {
      setMsg("Action failed.");
    }
  }

  return (
    <div className="max-w-4xl mx-auto px-6 py-10">
      <h1 className="text-3xl font-bold mb-8">Admin Dashboard</h1>

      {msg && (
        <div className="bg-primary/10 border border-primary/30 text-primary rounded-xl px-4 py-3 mb-6 text-sm">
          {msg}
        </div>
      )}

      {/* Tabs */}
      <div className="flex gap-2 mb-8">
        {["users", "pets"].map((t) => (
          <button
            key={t}
            onClick={() => setTab(t)}
            className={`px-6 py-2.5 rounded-xl font-medium capitalize transition ${
              tab === t
                ? "bg-primary text-white"
                : "border border-white/10 text-on-surface-variant hover:border-white/30"
            }`}
          >
            Pending {t}
          </button>
        ))}
      </div>

      {/* Users tab */}
      {tab === "users" && (
        <div className="flex flex-col gap-4">
          {users.length === 0 ? (
            <div className="text-center py-20 bg-surface rounded-2xl border border-white/10">
              <p className="text-on-surface-variant">No pending users</p>
            </div>
          ) : (
            users.map((u) => (
              <div
                key={u.id}
                className="bg-surface rounded-2xl p-5 border border-white/10 flex items-center justify-between"
              >
                <div>
                  <h3 className="font-bold">{u.name}</h3>
                  <p className="text-on-surface-variant text-sm">
                    {u.email} · {u.role}
                  </p>
                </div>
                <div className="flex gap-2">
                  <button
                    onClick={() => handleUser(u.id, "approve")}
                    className="px-4 py-2 bg-green-500/10 border border-green-500/30 text-green-400 rounded-xl text-sm hover:bg-green-500/20 transition"
                  >
                    Approve
                  </button>
                  <button
                    onClick={() => handleUser(u.id, "reject")}
                    className="px-4 py-2 bg-red-500/10 border border-red-500/30 text-red-400 rounded-xl text-sm hover:bg-red-500/20 transition"
                  >
                    Reject
                  </button>
                </div>
              </div>
            ))
          )}
        </div>
      )}

      {/* Pets tab */}
      {tab === "pets" && (
        <div className="flex flex-col gap-4">
          {pets.length === 0 ? (
            <div className="text-center py-20 bg-surface rounded-2xl border border-white/10">
              <p className="text-on-surface-variant">No pending pets</p>
            </div>
          ) : (
            pets.map((p) => (
              <div
                key={p.id}
                className="bg-surface rounded-2xl p-5 border border-white/10 flex items-center justify-between"
              >
                <div>
                  <h3 className="font-bold">{p.petName}</h3>
                  <p className="text-on-surface-variant text-sm">
                    {p.breed} · {p.type} · {p.location}
                  </p>
                </div>
                <div className="flex gap-2">
                  <button
                    onClick={() => handlePet(p.id, "approve")}
                    className="px-4 py-2 bg-green-500/10 border border-green-500/30 text-green-400 rounded-xl text-sm hover:bg-green-500/20 transition"
                  >
                    Approve
                  </button>
                  <button
                    onClick={() => handlePet(p.id, "reject")}
                    className="px-4 py-2 bg-red-500/10 border border-red-500/30 text-red-400 rounded-xl text-sm hover:bg-red-500/20 transition"
                  >
                    Reject
                  </button>
                </div>
              </div>
            ))
          )}
        </div>
      )}
    </div>
  );
}
