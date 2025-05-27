import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import api from '../../api/axios';
import type { PagedList, CourseResponse } from '../../types';

interface CoursesState {
  paged: PagedList<CourseResponse> | null;
  status: 'idle' | 'loading' | 'succeeded' | 'failed';
  error: string | null;
  coursesByIds: CourseResponse[] | null;
}

const initialState: CoursesState = {
  paged: null,
  status: 'idle',
  error: null,
  coursesByIds: null,
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


export const fetchCoursesByIds = createAsyncThunk(
  'courses/fetchByIds',
  async (ids: string[], { rejectWithValue }) => {
    try {
      const params = new URLSearchParams();
      ids.forEach(id => params.append('courseIds', id));
      const response = await api.get<CourseResponse[]>('/api/courses/ids', {
        params,
      });
      return response.data;
    } catch (err: any) {
      return rejectWithValue(err?.response?.data?.details || 'Failed to fetch courses by ids');
    }
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
      })
      .addCase(fetchCoursesByIds.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(fetchCoursesByIds.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.coursesByIds = action.payload;
      })
      .addCase(fetchCoursesByIds.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.payload as string || 'Failed to fetch courses by ids';
      });
  },
});

export default coursesSlice.reducer; 