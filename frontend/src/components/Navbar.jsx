import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { useEffect, useState, useRef } from "react";
import * as signalR from "@microsoft/signalr";

export default function Navbar() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const [notifications, setNotifications] = useState([]);
  const [showNotif, setShowNotif] = useState(false);
  const connRef = useRef(null);

  useEffect(() => {
    if (!user) return;

    async function connect() {
      // Create a fresh connection for this user session
      const conn = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:5073/hubs/notifications", {
          withCredentials: true,
        })
        .withAutomaticReconnect()
        .configureLogging(signalR.LogLevel.None)
        .build();

      connRef.current = conn;

      try {
        await conn.start();
        console.log("✅ Connected! State:", conn.state);

        // Join personal groups
        await conn.invoke("JoinGroup", `owner_${user.id}`);
        await conn.invoke("JoinGroup", `user_${user.id}`);
        console.log(`✅ Joined: owner_${user.id}, user_${user.id}`);

        if (user.role === "Admin") {
          await conn.invoke("JoinGroup", "admin_group");
          console.log("✅ Joined: admin_group");

          conn.on("NewUserPending", (data) => {
            console.log("🔔 NewUserPending:", data);
            setNotifications((prev) => [...prev, data.message]);
          });

          conn.on("NewPetPending", (data) => {
            console.log("🔔 NewPetPending:", data);
            setNotifications((prev) => [...prev, data.message]);
          });
        }

        // Listen for notifications
        if (user.role === "Shelter" || user.role === "PetOwner") {
          conn.on("NewRequest", (data) => {
            console.log("🔔 NewRequest:", data);
            setNotifications((prev) => [...prev, data.message]);
          });
        }

        if (user.role === "Adopter") {
          conn.on("RequestUpdated", (data) => {
            console.log("🔔 RequestUpdated:", data);
            setNotifications((prev) => [...prev, data.message]);
          });
        }
      } catch (err) {
        console.error("❌ SignalR failed:", err);
      }
    }

    connect();

    // Cleanup on logout/unmount
    return () => {
      if (connRef.current) {
        connRef.current.stop();
        connRef.current = null;
        console.log("🔌 SignalR disconnected");
      }
    };
  }, [user]);

  function handleLogout() {
    logout();
    navigate("/");
  }

  return (
    <nav className="fixed top-0 z-50 w-full bg-surface/90 backdrop-blur-md border-b border-white/10">
      <div className="max-w-7xl mx-auto px-8 py-5 flex justify-between items-center">
        <Link to="/" className="flex items-center gap-2">
          <span className="material-symbols-outlined text-primary text-4xl">
            pets
          </span>
          <span className="font-bold text-3xl tracking-tight text-on-background">
            PetAdopt
          </span>
        </Link>

        <div className="hidden md:flex items-center gap-8 text-lg">
          <Link to="/" className="hover:text-primary transition-colors">
            Home
          </Link>
          <Link to="/browse" className="hover:text-primary transition-colors">
            Browse
          </Link>

          {user?.role === "Adopter" && (
            <>
              <Link
                to="/favorites"
                className="hover:text-primary transition-colors"
              >
                Favorites
              </Link>
              <Link
                to="/my-requests"
                className="hover:text-primary transition-colors"
              >
                My Requests
              </Link>
            </>
          )}

          {(user?.role === "Shelter" || user?.role === "PetOwner") && (
            <>
              <Link
                to="/my-pets"
                className="hover:text-primary transition-colors"
              >
                My Pets
              </Link>
              <Link
                to="/requests"
                className="hover:text-primary transition-colors"
              >
                Requests
              </Link>
            </>
          )}

          {user?.role === "Admin" && (
            <>
              <Link
                to="/admin"
                className="hover:text-primary transition-colors"
              >
                Admin
              </Link>
            </>
          )}
        </div>

        <div className="flex items-center gap-4">
          {user ? (
            <div className="flex items-center gap-4">
              <div className="relative">
                <button
                  onClick={() => setShowNotif(!showNotif)}
                  className="relative p-2 rounded-full hover:bg-white/10 transition"
                >
                  <span className="material-symbols-outlined text-2xl">
                    notifications
                  </span>
                  {notifications.length > 0 && (
                    <span className="absolute top-0 right-0 w-5 h-5 bg-red-500 text-white text-xs rounded-full flex items-center justify-center">
                      {notifications.length}
                    </span>
                  )}
                </button>

                {showNotif && (
                  <div className="absolute right-0 top-12 w-80 bg-surface border border-white/10 rounded-2xl shadow-xl z-50 overflow-hidden">
                    <div className="flex items-center justify-between px-4 py-3 border-b border-white/10">
                      <span className="font-semibold">Notifications</span>
                      <button
                        onClick={() => setNotifications([])}
                        className="text-xs text-on-surface-variant hover:text-primary"
                      >
                        Clear all
                      </button>
                    </div>
                    {notifications.length === 0 ? (
                      <p className="text-on-surface-variant text-sm text-center py-6">
                        No notifications
                      </p>
                    ) : (
                      <div className="max-h-64 overflow-y-auto">
                        {notifications.map((n, i) => (
                          <div
                            key={i}
                            className="px-4 py-3 border-b border-white/5 text-sm hover:bg-white/5"
                          >
                            🔔 {n}
                          </div>
                        ))}
                      </div>
                    )}
                  </div>
                )}
              </div>

              <span className="text-sm text-on-surface-variant hidden md:block">
                Hi, {user.name}
              </span>
              <button
                onClick={handleLogout}
                className="px-6 py-2.5 rounded-full border border-red-500/30 text-red-400 hover:bg-red-500/10 transition"
              >
                Logout
              </button>
            </div>
          ) : (
            <div className="flex items-center gap-3">
              <Link
                to="/login"
                className="px-6 py-2.5 rounded-full border border-white/30 hover:bg-white/10 transition"
              >
                Login
              </Link>
              <Link
                to="/register"
                className="px-6 py-2.5 rounded-full bg-primary text-white hover:bg-primary/90 transition"
              >
                Register
              </Link>
            </div>
          )}
        </div>
      </div>
    </nav>
  );
}
