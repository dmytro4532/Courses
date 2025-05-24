import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import api from '../../api/axios';
import type { PagedList } from '../../types';

export interface CourseProgress {
  id: string;
  courseId: string;
  userId: string;
  startedAt: string;
  completedAt?: string;
  progressPercents: number,
  totalTopics: number,
  completedTopics: number,
}

interface ProgressesState {
  paged: PagedList<CourseProgress> | null;
  progress: CourseProgress | null;
  status: 'idle' | 'loading' | 'succeeded' | 'failed';
  error: string | null;
}

const initialState: ProgressesState = {
  paged: null,
  progress: null,
  status: 'idle',
  error: null,
};

export const fetchProgresses = createAsyncThunk(
  'progresses/fetchAll',
  async (
    params: { userId?: string; pageIndex?: number; pageSize?: number; orderBy?: string; orderDirection?: string } = {},
    { rejectWithValue }
  ) => {
    try {
      const res = await api.get<PagedList<CourseProgress>>('/api/coursesprogresses', {
        params: {
          UserId: params.userId,
          PageIndex: params.pageIndex ?? 0,
          PageSize: params.pageSize ?? 10,
          OrderBy: params.orderBy ?? 'Id',
          OrderDirection: params.orderDirection ?? 'ASC',
        },
      });
      return res.data;
    } catch (err: any) {
      return rejectWithValue(err?.response?.data?.details || 'Failed to fetch progresses');
    }
  }
);

export const fetchProgressByCourseId = createAsyncThunk(
  'progresses/fetchByCourseId',
  async (courseId: string, { rejectWithValue }) => {
    try {
      const res = await api.get<CourseProgress>(`/api/coursesprogresses/${courseId}`);
      return res.data;
    } catch (err: any) {
      return rejectWithValue(err?.response?.data?.details || 'Failed to fetch progress');
    }
  }
);

export const startCourseProgress = createAsyncThunk(
  'progresses/startCourse',
  async (courseId: string, { rejectWithValue }) => {
    try {
      const res = await api.post(`/api/coursesprogresses/${courseId}/start`);
      return res.data;
    } catch (err: any) {
      return rejectWithValue(err?.response?.data?.details || 'Failed to start course');
    }
  }
);

export const completeCourseProgress = createAsyncThunk(
  'progresses/completeCourse',
  async (courseId: string, { rejectWithValue }) => {
    try {
      const res = await api.post(`/api/coursesprogresses/${courseId}/complete`);
      return res.data;
    } catch (err: any) {
      return rejectWithValue(err?.response?.data?.details || 'Failed to complete course');
    }
  }
);

export const removeCourseProgress = createAsyncThunk(
  'progresses/removeCourseProgress',
  async (courseId: string, { rejectWithValue }) => {
    try {
      await api.delete(`/api/coursesprogresses/${courseId}`);
      return courseId;
    } catch (err: any) {
      return rejectWithValue(err?.response?.data?.details || 'Failed to reset course progress');
    }
  }
);

const progressesSlice = createSlice({
  name: 'progresses',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(fetchProgresses.pending, (state) => {
        state.status = 'loading';
        state.error = null;
      })
      .addCase(fetchProgresses.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.paged = action.payload;
      })
      .addCase(fetchProgresses.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.payload as string;
      })
      .addCase(fetchProgressByCourseId.pending, (state) => {
        state.status = 'loading';
        state.error = null;
      })
      .addCase(fetchProgressByCourseId.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.progress = action.payload;
      })
      .addCase(fetchProgressByCourseId.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.payload as string;
        state.progress = null;
      })
      .addCase(startCourseProgress.pending, (state) => {
        state.status = 'loading';
        state.error = null;
      })
      .addCase(startCourseProgress.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.progress = action.payload;
      })
      .addCase(startCourseProgress.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.payload as string;
      })
      .addCase(completeCourseProgress.pending, (state) => {
        state.status = 'loading';
        state.error = null;
      })
      .addCase(completeCourseProgress.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.progress = action.payload;
      })
      .addCase(completeCourseProgress.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.payload as string;
      })
      .addCase(removeCourseProgress.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(removeCourseProgress.fulfilled, (state, action) => {
        state.status = 'succeeded';
        if (state.progress && state.progress.courseId === action.payload) {
          state.progress = null;
        }
      })
      .addCase(removeCourseProgress.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.payload as string;
      });
  },
});

export default progressesSlice.reducer;