import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { requestApi } from "../api/requestApi";

const statusColors = {
  Pending: "bg-yellow-500/10 text-yellow-400",
  Accepted: "bg-green-500/10 text-green-400",
  Rejected: "bg-red-500/10 text-red-400",
};

export default function MyRequests() {
  const [requests, setRequests] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    requestApi
      .getMine()
      .then((res) => setRequests(res.data))
      .catch(() => setRequests([]))
      .finally(() => setLoading(false));
  }, []);

  if (loading)
    return (
      <div className="min-h-[60vh] flex items-center justify-center">
        <p className="text-on-surface-variant text-xl">Loading...</p>
      </div>
    );

  return (
    <div className="max-w-4xl mx-auto px-6 py-10">
      <h1 className="text-3xl font-bold mb-8">My Adoption Requests</h1>

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
              <div className="flex items-center justify-between">
                <div>
                  <h3 className="font-bold text-lg">{req.petName}</h3>
                  <p className="text-on-surface-variant text-sm">
                    {new Date(req.createdAt).toLocaleDateString()}
                  </p>
                </div>
                <div className="flex items-center gap-3">
                  <span
                    className={`px-3 py-1 rounded-full text-sm font-medium ${statusColors[req.status] || "bg-gray-500/10 text-gray-400"}`}
                  >
                    {req.status}
                  </span>

                  {/* ✅ Leave Review button for accepted requests */}
                  {req.status === "Accepted" && (
                    <Link
                      to={`/review/${req.ownerId}`}
                      className="px-4 py-2 bg-primary/10 border border-primary/30 text-primary rounded-xl text-sm hover:bg-primary/20 transition"
                    >
                      ⭐ Leave Review
                    </Link>
                  )}
                </div>
              </div>

              {/* Show adoption history if exists */}
              {req.adoptionHistory && (
                <div className="mt-3 bg-background rounded-xl p-3 border border-white/5">
                  <p className="text-xs text-on-surface-variant mb-1">
                    Adoption history sent
                  </p>
                  <p className="text-sm text-on-surface-variant">
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
