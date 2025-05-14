import { configureStore } from '@reduxjs/toolkit';
import coursesReducer from './slices/coursesSlice';
import authReducer from './slices/authSlice';
import topicsReducer from './slices/topicsSlice';

export const store = configureStore({
  reducer: {
    courses: coursesReducer,
    auth: authReducer,
    topics: topicsReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch; 