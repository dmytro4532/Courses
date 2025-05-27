import { Alert, Button, Card, Space, Typography, Progress, Tag } from 'antd';
import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Link, useParams } from 'react-router-dom';
import LoadingSpinner from '../components/common/LoadingSpinner';
import CourseHeader from '../components/features/courses/CourseHeader';
import type { AppDispatch, RootState } from '../store';
import { fetchCourseById } from '../store/slices/courseSlice';
import {
  fetchProgressByCourseId,
  startCourseProgress,
  completeCourseProgress,
  removeCourseProgress, // <-- import the new thunk
} from '../store/slices/progressesSlice';

const { Paragraph } = Typography;

const CourseDetails = () => {
  const { id } = useParams<{ id: string }>();
  const dispatch = useDispatch<AppDispatch>();
  const { course, loading, error } = useSelector((state: RootState) => state.course);
  const { progress, status: progressStatus } = useSelector((state: RootState) => state.progresses);

  useEffect(() => {
    if (id) {
      dispatch(fetchCourseById(id));
      dispatch(fetchProgressByCourseId(id));
    }
  }, [id, dispatch]);

  const handleStartCourse = () => {
    if (id) {
      dispatch(startCourseProgress(id)).then(() => {
        dispatch(fetchProgressByCourseId(id));
      });
    }
  };

  const handleCompleteCourse = () => {
    if (id) {
      dispatch(completeCourseProgress(id)).then(() => {
        dispatch(fetchProgressByCourseId(id));
      });
    }
  };

  const handleRemoveProgress = () => {
    if (id && progress) {
      dispatch(removeCourseProgress(id)).then(() => {
        dispatch(fetchProgressByCourseId(id));
      });
    }
  };

  let courseStatus: React.ReactNode = <Tag color="default">Not started</Tag>;
  if (progress) {
    if (progress.completedAt) {
      courseStatus = <Tag color="green">Completed</Tag>;
    } else {
      courseStatus = <Tag color="blue">Started</Tag>;
    }
  }

  if (loading) {
    return <LoadingSpinner />;
  }

  if (error || !course) {
    return <Alert type="error" message={error || 'Course not found'} style={{ marginTop: 48 }} />;
  }

  return (
    <div style={{ padding: '32px 0', maxWidth: 900, margin: '0 auto' }}>
      <Space direction="vertical" size={32} style={{ width: '100%' }}>
        <CourseHeader
          title={course.title}
          imageUrl={course.imageUrl}
          createdAt={course.createdAt}
        />
        <Card style={{ textAlign: 'left', width: '100%', marginTop: 24 }}>
          <div style={{ marginBottom: 12 }}>
            <b>Status:</b> {courseStatus}
          </div>
          <Paragraph style={{ fontSize: 16 }}>{course.description}</Paragraph>
        </Card>
        {progress && (
          <div style={{ marginTop: 24 }}>
            <Progress
              percent={progress.progressPercents}
              status={progress.progressPercents === 100 ? 'success' : 'active'}
              format={percent => `${percent}% (${progress.completedTopics}/${progress.totalTopics} topics)`}
            />
          </div>
        )}
        <Card style={{ marginTop: 24, width: '100%', textAlign: 'left' }}>
          <Space>
            {!progress && (
              <Button
                type="primary"
                loading={progressStatus === 'loading'}
                onClick={handleStartCourse}
              >
                Start Course
              </Button>
            )
            }
            {progress && progress.progressPercents === 100 && !progress.completedAt && (
              <Button
                type="primary"
                loading={progressStatus === 'loading'}
                onClick={handleCompleteCourse}
              >
                Complete Course
              </Button>
            )}
            <Link to={`/courses/${id}/topics`}>
              <Button type="primary">View Topics</Button>
            </Link>
            {progress && (
              <Button
                danger
                loading={progressStatus === 'loading'}
                onClick={handleRemoveProgress}
              >
                Reset Progress
              </Button>
            )}
          </Space>
        </Card>
      </Space>
    </div>
  );
};

export default CourseDetails;