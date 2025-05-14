import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import api from '../../api/axios';
import type { PagedList, CourseResponse } from '../../types';

interface CoursesState {
  paged: PagedList<CourseResponse> | null;
  status: 'idle' | 'loading' | 'succeeded' | 'failed';
  error: string | null;
}

const initialState: CoursesState = {
  paged: null,
  status: 'idle',
  error: null,
};

export const fetchCourses = createAsyncThunk(
  'courses/fetchCourses',
  async (params: { pageIndex?: number; pageSize?: number; orderBy?: string; orderDirection?: string } = {}) => {
    const response = await api.get<PagedList<CourseResponse>>('/api/courses', {
      params: {
        PageIndex: params.pageIndex ?? 0,
        PageSize: params.pageSize ?? 10,
        OrderBy: params.orderBy ?? 'Id',
        OrderDirection: params.orderDirection ?? 'ASC',
      },
    });
    return response.data;
  }
);

const coursesSlice = createSlice({
  name: 'courses',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchCourses.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(fetchCourses.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.paged = action.payload;
      })
      .addCase(fetchCourses.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message || 'Failed to fetch courses';
      });
  },
});

export default coursesSlice.reducer; 