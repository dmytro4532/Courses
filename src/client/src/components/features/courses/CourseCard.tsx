import { Card, Typography } from 'antd';
import { Link } from 'react-router-dom';
import { formatDate } from '../../../utils/date';

const { Meta } = Card;
const { Paragraph } = Typography;

interface CourseCardProps {
  id: string;
  title: string;
  description: string;
  imageUrl?: string;
  createdAt: string;
}

const placeholder = 'https://upload.wikimedia.org/wikipedia/commons/8/86/Solid_grey.svg';

const CourseCard = ({ id, title, description, imageUrl, createdAt }: CourseCardProps) => {
  return (
    <Card
      hoverable
      cover={<img alt={title} src={imageUrl || placeholder} style={{ height: 180, objectFit: 'cover' }} />}
      actions={[
        <Link to={`/courses/${id}`}>View Details</Link>
      ]}
      styles={{ body: { textAlign: 'left' } }}
    >
      <Meta title={title} description={description} />
      <Paragraph type="secondary">{formatDate(createdAt)}</Paragraph>
    </Card>
  );
};

export default CourseCard; 