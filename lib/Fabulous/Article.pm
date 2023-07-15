
package Fabulous::Article;
use Moo;

has title => (is => 'ro');
has href  => (is => 'ro');
has tags  => (is => 'ro', default => sub { +[] });

1;

=head1 NAME

lib::Fabulous::Article.pm - Storage for the article

=head1 SYNOPSIS

    my $article = Fabulous::Article->new(title => 'This', href => 'http://someurl', tags => [qw(one two three)]);

=head1 DESCRIPTION

Simple storage for the article

=cut
