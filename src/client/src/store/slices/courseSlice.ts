import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import api from '../../api/axios';
import type { CourseResponse } from '../../types';

export const fetchCourseById = createAsyncThunk<CourseResponse, string>(
  'course/fetchById',
  async (id, { rejectWithValue }) => {
    try {
      const res = await api.get<CourseResponse>(`/api/courses/${id}`);
      return res.data;
    } catch (err: any) {
      return rejectWithValue(err?.response?.data?.details || 'Failed to load course');
    }
  }
);

interface CourseState {
  course: CourseResponse | null;
  loading: boolean;
  error: string | null;
}

const initialState: CourseState = {
  course: null,
  loading: false,
  error: null,
};

const courseSlice = createSlice({
  name: 'course',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchCourseById.pending, (state) => {
        state.loading = true;
        state.error = null;
        state.course = null;
      })
      .addCase(fetchCourseById.fulfilled, (state, action) => {
        state.loading = false;
        state.course = action.payload;
      })
      .addCase(fetchCourseById.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
      });
  },
});

export default courseSlice.reducer;