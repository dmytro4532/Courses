import { Provider } from 'react-redux';
import { Route, BrowserRouter as Router, Routes } from 'react-router-dom';
import './App.css';
import MainLayout from './components/layout/MainLayout';
import ProtectedRoute from './components/ProtectedRoute';
import CourseDetails from './pages/CourseDetails';
import Courses from './pages/Courses';
import Home from './pages/Home';
import Login from './pages/Login';
import Profile from './pages/Profile';
import Register from './pages/Register';
import TestDetails from './pages/TestDetails';
import { TestAttempt } from './pages/TestAttempt';
import { TestAttemptReview } from './pages/TestAttemptReview';
import Topics from './pages/Topics';
import { store } from './store';

function App() {
  return (
    <Provider store={store}>
      <Router>
        <Routes>
          <Route element={<MainLayout />}>
            <Route path="/" element={<Home />} />
            <Route path="/courses" element={<Courses />} />
            <Route path="/courses/:id" element={<CourseDetails />} />
            <Route path="/courses/:id/topics" element={<Topics />} />
            <Route path="/tests/:testId" element={<TestDetails />} />
            <Route path="/test-attempts/:attemptId" element={
              <ProtectedRoute>
                <TestAttempt />
              </ProtectedRoute>
            } />
            <Route path="/test-attempts/:attemptId/review" element={
              <ProtectedRoute>
                <TestAttemptReview />
              </ProtectedRoute>
            } />
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            <Route path="/profile" element={
              <ProtectedRoute>
                <Profile />
              </ProtectedRoute>
            } />
          </Route>
        </Routes>
      </Router>
    </Provider>
  );
}

export default App;
