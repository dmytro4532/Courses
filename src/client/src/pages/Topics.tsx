import { Col, Empty, Pagination, Row, Typography } from 'antd';
import { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useParams } from 'react-router-dom';
import api from '../api/axios';
import ErrorMessage from '../components/common/ErrorMessage';
import LoadingSpinner from '../components/common/LoadingSpinner';
import TopicCard from '../components/features/topics/TopicCard';
import type { AppDispatch, RootState } from '../store';
import { completeTopic, fetchCompletedTopics } from '../store/slices/completedTopicsSlice';
import { fetchProgressByCourseId } from '../store/slices/progressesSlice'; // <-- import progress fetch
import { fetchTopics } from '../store/slices/topicsSlice';
import type { CourseResponse } from '../types';

const { Title } = Typography;

const Topics = () => {
  const { id: courseId } = useParams<{ id: string }>();
  const dispatch = useDispatch<AppDispatch>();
  const { paged, status, error } = useSelector((state: RootState) => state.topics);
  const [page, setPage] = useState(1);
  const pageSize = 9;

  const [course, setCourse] = useState<CourseResponse | null>(null);
  const [courseLoading, setCourseLoading] = useState(true);
  const [courseError, setCourseError] = useState<string | null>(null);

  const { completed: completedTopicIds } = useSelector((state: RootState) => state.completedTopics);
  const { progress } = useSelector((state: RootState) => state.progresses);

  useEffect(() => {
    if (!courseId) return;
    setCourseLoading(true);
    setCourseError(null);
    api.get<CourseResponse>(`/api/courses/${courseId}`)
      .then((res) => setCourse(res.data))
      .catch((err) => setCourseError(err?.response?.data?.details || 'Failed to load course'))
      .finally(() => setCourseLoading(false));
  }, [courseId]);

  useEffect(() => {
    if (courseId && (status === 'idle' || page !== (paged?.pageIndex ?? 0) + 1)) {
      dispatch(fetchTopics({ courseId, pageIndex: page - 1, pageSize }));
    }
  }, [status, dispatch, page, paged?.pageIndex, courseId]);

  useEffect(() => {
    if (paged?.items.length) {
      dispatch(fetchCompletedTopics(paged?.items.map((t) => t.id) ?? []));
    }
  }, [dispatch, paged?.items]);

  useEffect(() => {
    if (courseId) {
      dispatch(fetchProgressByCourseId(courseId));
    }
  }, [dispatch, courseId]);

  const handleCompleteTopic = (topicId: string) => {
    dispatch(completeTopic(topicId))
      .then(() =>
        dispatch(fetchCompletedTopics(paged?.items.map((t) => t.id) ?? [])));
  };

  const topics = paged?.items ?? [];

  if (courseLoading) {
    return <LoadingSpinner />;
  }

  if (courseError || !course) {
    return <ErrorMessage message={courseError || 'Course not found'} />;
  }

  if (status === 'loading') {
    return <LoadingSpinner />;
  }

  if (status === 'failed') {
    return <ErrorMessage message={error || 'Failed to load topics'} />;
  }

  if (!Array.isArray(topics) || topics.length === 0) {
    return <Empty description="No topics found" style={{ marginTop: 48 }} />;
  }

  const courseStarted = !!progress;

  return (
    <div style={{ padding: '24px 0' }}>
      <Title level={2}>{course.title}</Title>
      <Row gutter={[16, 16]}>
        {topics.map((topic) => {
          const isCompleted = completedTopicIds.includes(topic.id);
          return (
            <Col xs={24} key={topic.id}>
              <TopicCard
                topic={topic}
                isCompleted={isCompleted}
                onComplete={() => handleCompleteTopic(topic.id)}
                canComplete={!isCompleted && courseStarted}
              />
            </Col>
          );
        })}
      </Row>
      <div style={{ marginTop: 32, textAlign: 'center' }}>
        <Pagination
          current={page}
          pageSize={pageSize}
          total={paged?.totalCount ?? 0}
          onChange={setPage}
          showSizeChanger={false}
        />
      </div>
    </div>
  );
};

export default Topics;