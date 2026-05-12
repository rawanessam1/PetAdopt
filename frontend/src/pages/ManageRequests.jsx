import { useEffect, useState } from "react";
import { requestApi } from "../api/requestApi";

export default function ManageRequests() {
  const [requests, setRequests] = useState([]);
  const [loading, setLoading] = useState(true);
  const [msg, setMsg] = useState("");

  useEffect(() => {
    requestApi
      .getForOwner()
      .then((res) => setRequests(res.data))
      .catch(() => setRequests([]))
      .finally(() => setLoading(false));
  }, []);

  async function handle(id, action) {
    try {
      if (action === "accept") await requestApi.accept(id);
      else await requestApi.reject(id);
      setRequests((prev) =>
        prev.map((r) =>
          r.id === id
            ? { ...r, status: action === "accept" ? "Accepted" : "Rejected" }
            : r,
        ),
      );
      setMsg(
        `Request ${action === "accept" ? "accepted" : "rejected"} successfully.`,
      );
    } catch {
      setMsg("Action failed.");
    }
  }

  if (loading)
    return (
      <div className="min-h-[60vh] flex items-center justify-center">
        <p className="text-on-surface-variant text-xl">Loading...</p>
      </div>
    );

  return (
    <div className="max-w-4xl mx-auto px-6 py-10">
      <h1 className="text-3xl font-bold mb-8">Manage Adoption Requests</h1>

      {msg && (
        <div className="bg-primary/10 border border-primary/30 text-primary rounded-xl px-4 py-3 mb-6 text-sm">
          {msg}
        </div>
      )}

      {requests.length === 0 ? (
        <div className="text-center py-20 bg-surface rounded-2xl border border-white/10">
          <p className="text-on-surface-variant text-lg">No requests yet</p>
        </div>
      ) : (
        <div className="flex flex-col gap-4">
          {requests.map((req) => (
            <div
              key={req.id}
              className="bg-surface rounded-2xl p-5 border border-white/10"
            >
              <div className="flex items-center justify-between mb-3">
                <div>
                  <h3 className="font-bold text-lg">{req.petName}</h3>
                  <p className="text-on-surface-variant text-sm">
                    Requested on {new Date(req.createdAt).toLocaleDateString()}
                  </p>
                </div>
                {req.status === "Pending" ? (
                  <div className="flex gap-2">
                    <button
                      onClick={() => handle(req.id, "accept")}
                      className="px-4 py-2 bg-green-500/10 border border-green-500/30 text-green-400 rounded-xl text-sm hover:bg-green-500/20 transition"
                    >
                      Accept
                    </button>
                    <button
                      onClick={() => handle(req.id, "reject")}
                      className="px-4 py-2 bg-red-500/10 border border-red-500/30 text-red-400 rounded-xl text-sm hover:bg-red-500/20 transition"
                    >
                      Reject
                    </button>
                  </div>
                ) : (
                  <span
                    className={`px-3 py-1 rounded-full text-sm font-medium ${
                      req.status === "Accepted"
                        ? "bg-green-500/10 text-green-400"
                        : "bg-red-500/10 text-red-400"
                    }`}
                  >
                    {req.status}
                  </span>
                )}
              </div>

              {/* ✅ Show adoption history */}
              {req.adoptionHistory && (
                <div className="mt-3 bg-background rounded-xl p-4 border border-white/5">
                  <p className="text-xs text-on-surface-variant mb-1 font-medium">
                    Adopter's History
                  </p>
                  <p className="text-sm text-on-surface-variant leading-relaxed">
                    {req.adoptionHistory}
                  </p>
                </div>
              )}
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
