
package Fabulous::Article;
use Moo;

has title => (is => 'ro');
has href  => (is => 'ro');
has tags  => (is => 'ro', default => sub { +[] });

1;
