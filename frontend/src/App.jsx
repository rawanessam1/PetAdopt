import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Navbar from "./components/Navbar";
import ProtectedRoute from "./components/ProtectedRoute";

import Home from "./pages/Home";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Browse from "./pages/Browse";
import PetDetails from "./pages/PetDetails";
import Favorites from "./pages/Favorites";
import MyRequests from "./pages/MyRequests";
import MyPets from "./pages/MyPets";
import PetForm from "./pages/PetForm";
import ManageRequests from "./pages/ManageRequests";
import Admin from "./pages/Admin";
import Review from "./pages/Review";

function App() {
  return (
    <Router>
      <div className="dark min-h-screen bg-background text-on-background">
        <Navbar />
        <main className="pt-20">
          <Routes>
            {/* Public */}
            <Route path="/" element={<Home />} />
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            <Route path="/browse" element={<Browse />} />
            <Route path="/pets/:id" element={<PetDetails />} />

            {/* Adopter only */}
            <Route element={<ProtectedRoute allowedRoles={["Adopter"]} />}>
              <Route path="/favorites" element={<Favorites />} />
              <Route path="/my-requests" element={<MyRequests />} />
              <Route path="/review/:id" element={<Review />} />
            </Route>

            {/* Shelter / PetOwner only */}
            <Route
              element={
                <ProtectedRoute allowedRoles={["Shelter", "PetOwner"]} />
              }
            >
              <Route path="/my-pets" element={<MyPets />} />
              <Route path="/my-pets/new" element={<PetForm />} />
              <Route path="/my-pets/:id/edit" element={<PetForm />} />
              <Route path="/requests" element={<ManageRequests />} />
            </Route>

            {/* Admin only */}
            <Route element={<ProtectedRoute allowedRoles={["Admin"]} />}>
              <Route path="/admin" element={<Admin />} />
            </Route>

            {/* 404 */}
            <Route
              path="*"
              element={
                <div className="min-h-[60vh] flex items-center justify-center">
                  <div className="text-center">
                    <h1 className="text-5xl font-bold mb-4">404</h1>
                    <p className="text-on-surface-variant text-xl">
                      Page Not Found
                    </p>
                  </div>
                </div>
              }
            />
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;
