import { BrowserRouter as Router } from "react-router-dom";
import { AppProvider } from "./context/AppContext";
import { ScrollToTop } from "./components/common/ScrollToTop";
import "./index.css";
import AppContent from "./components/AppContent";

function App() {
  return (
    <Router>
      <ScrollToTop />
      <AppProvider>
        <AppContent />
      </AppProvider>
    </Router>
  );
}

export default App;
