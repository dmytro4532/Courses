import { Alert, Card, Space, Typography, Button } from 'antd';
import { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import api from '../api/axios';
import LoadingSpinner from '../components/common/LoadingSpinner';
import CourseHeader from '../components/features/courses/CourseHeader';
import type { CourseResponse } from '../types';

const { Paragraph } = Typography;

const CourseDetails = () => {
  const { id } = useParams<{ id: string }>();
  const [course, setCourse] = useState<CourseResponse | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!id || course) return;

    setLoading(true);
    setError(null);

    api.get<CourseResponse>(`/api/courses/${id}`)
      .then((courseRes) => {
        setCourse(courseRes.data);
      })
      .catch((err) => {
        setError(err?.response?.data?.details || 'Failed to load course');
      })
      .finally(() => {
        setLoading(false);
      });
  }, [id, course]);

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
        <Card style={{ textAlign: 'left' }}>
          <Paragraph style={{ fontSize: 16 }}>{course.description}</Paragraph>
        </Card>
        <div style={{ marginTop: 24 }}>
          <Link to={`/courses/${id}/topics`}>
            <Button type="primary">View Topics</Button>
          </Link>
        </div>
      </Space>
    </div>
  );
};

export default CourseDetails;