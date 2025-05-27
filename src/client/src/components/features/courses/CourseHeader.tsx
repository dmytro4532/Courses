import { Typography, Image, Space } from 'antd';
import { formatDate } from '../../../utils/date';

const { Title, Paragraph } = Typography;
const placeholder = 'https://upload.wikimedia.org/wikipedia/commons/8/86/Solid_grey.svg';

interface CourseHeaderProps {
  title: string;
  imageUrl?: string;
  createdAt: string;
}

const CourseHeader = ({ title, imageUrl, createdAt }: CourseHeaderProps) => {
  return (
    <div style={{ textAlign: 'center' }}>
      <Image
        src={imageUrl || placeholder}
        alt={title}
        style={{ maxHeight: 500, objectFit: 'cover', borderRadius: 12, margin: '0 auto' }}
        preview={false}
        fallback={placeholder}
      />
      <Title level={1} style={{ marginBottom: 8 }}>{title}</Title>
      <Paragraph type="secondary" style={{ marginTop: 12 }}>
        {formatDate(createdAt)}
      </Paragraph>
    </div>
  );
};

export default CourseHeader; 