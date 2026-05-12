import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import api from "../api/axiosInstance";

export default function Review() {
  const { id } = useParams(); // ownerId or petId
  const { user } = useAuth();
  const navigate = useNavigate();

  const [owner, setOwner] = useState(null);
  const [rating, setRating] = useState(5);
  const [comment, setComment] = useState("");
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [success, setSuccess] = useState(false);

  useEffect(() => {
    // Fetch owner info (you can adjust endpoint if needed)
    api
      .get(`/users/${id}`)
      .then((res) => setOwner(res.data))
      .catch(() => {})
      .finally(() => setLoading(false));
  }, [id]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!comment.trim()) return;

    setSubmitting(true);
    try {
      await api.post("/reviews", {
        ownerId: parseInt(id),
        rating,
        comment: comment.trim(),
      });

      setSuccess(true);
      setTimeout(() => {
        navigate("/my-requests"); // or wherever you want to redirect
      }, 2000);
    } catch (err) {
      alert("Failed to submit review. Please try again.");
    } finally {
      setSubmitting(false);
    }
  };

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        Loading...
      </div>
    );
  }

  if (success) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gradient-to-b from-primary/5 to-transparent">
        <div className="text-center">
          <div className="text-6xl mb-4">🎉</div>
          <h2 className="text-3xl font-bold mb-2">Thank You!</h2>
          <p className="text-on-surface-variant">
            Your review has been submitted successfully.
          </p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-background py-12">
      <div className="max-w-2xl mx-auto px-6">
        <button
          onClick={() => navigate(-1)}
          className="text-on-surface-variant hover:text-primary mb-8 flex items-center gap-2"
        >
          ← Back
        </button>

        <div className="bg-surface rounded-3xl p-8 shadow-lg">
          <h1 className="text-3xl font-bold mb-2">Leave a Review</h1>
          <p className="text-on-surface-variant mb-8">
            Share your experience with {owner?.name || "the owner"}
          </p>

          <form onSubmit={handleSubmit} className="space-y-8">
            {/* Rating */}
            <div>
              <label className="block text-sm text-on-surface-variant mb-3">
                How would you rate your experience?
              </label>
              <div className="flex gap-3 text-4xl">
                {[1, 2, 3, 4, 5].map((star) => (
                  <button
                    key={star}
                    type="button"
                    onClick={() => setRating(star)}
                    className={`transition-transform hover:scale-110 ${star <= rating ? "text-yellow-400" : "text-gray-300"}`}
                  >
                    ★
                  </button>
                ))}
              </div>
              <p className="text-sm text-on-surface-variant mt-2">
                {rating} out of 5 stars
              </p>
            </div>

            {/* Comment */}
            <div>
              <label className="block text-sm text-on-surface-variant mb-2">
                Your Review <span className="text-red-400">*</span>
              </label>
              <textarea
                value={comment}
                onChange={(e) => setComment(e.target.value)}
                required
                rows={6}
                placeholder="Tell others about your experience with this owner and their pet..."
                className="w-full bg-background border border-white/10 rounded-2xl px-5 py-4 focus:border-primary resize-none"
              />
            </div>

            <button
              type="submit"
              disabled={submitting || !comment.trim()}
              className="w-full bg-primary text-white py-4 rounded-2xl font-semibold text-lg hover:bg-primary/90 transition disabled:opacity-50"
            >
              {submitting ? "Submitting..." : "Submit Review"}
            </button>
          </form>
        </div>
      </div>
    </div>
  );
}
