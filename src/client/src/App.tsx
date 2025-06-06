import { Provider } from 'react-redux';
import { Route, BrowserRouter as Router, Routes, Navigate } from 'react-router-dom';
import './App.css';
import MainLayout from './components/layout/MainLayout';
import ProtectedRoute from './components/ProtectedRoute';
import AdminRoute from './components/AdminRoute';
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
import AdminCourses from './pages/admin/AdminCourses';
import AdminTopics from './pages/admin/AdminTopics';
import AdminTests from './pages/admin/AdminTests';
import AdminQuestions from './pages/admin/AdminQuestions';
import AdminUsers from './pages/admin/AdminUsers';
import { store } from './store';
import { SnackbarProvider } from 'notistack';

function App() {
  return (
    <Provider store={store}>
      <SnackbarProvider />
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
          <Route path="/admin" element={<AdminRoute />}>
            <Route index element={<Navigate to="/admin/courses" replace />} />
            <Route path="courses" element={<AdminCourses />} />
            <Route path="topics/:courseId" element={<AdminTopics />} />
            <Route path="tests" element={<AdminTests />} />
            <Route path="questions/:testId" element={<AdminQuestions />} />
            <Route path="users" element={<AdminUsers />} />
          </Route>
        </Routes>
      </Router>
    </Provider>
  );
}

export default App;
